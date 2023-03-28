using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using XboxCsMgr.Helpers.Http;
using XboxCsMgr.Helpers.Serialization;
using XboxCsMgr.XboxLive.Model.TitleStorage;

namespace XboxCsMgr.XboxLive.Services
{
    public class TitleStorageService : XboxLiveService
    {
        private string ServiceConfigurationId;
        private string PackageFamilyName;

        private const int MinBlockUploadSize = 1024;
        private const int MaxBlockUploadSize = 4 * 1024 * 1024;
        private const int DefaultBlockUploadSize = 256 * 1024;

        public TitleStorageService(XboxLiveConfig config) : base(config, "https://titlestorage.xboxlive.com")
        {
            HttpHeaders = new Dictionary<string, string>()
            {
                { "x-xbl-contract-version", "107" }
            };
        }

        public TitleStorageService(XboxLiveConfig config, string pfn, string scid)
            : base(config, "https://titlestorage.xboxlive.com")
        {
            ServiceConfigurationId = scid;
            PackageFamilyName = pfn;

            HttpHeaders = new Dictionary<string, string>()
            {
                { "x-xbl-contract-version", "107" },
                { "x-xbl-pfn", PackageFamilyName }
            };
        }

        /// <summary>
        /// Obtain a lock for specified title connected storage
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <returns></returns>
        public Task<TitleStorageQuota> AssignLock()
        {
            // PUT
            return SignAndRequest<TitleStorageQuota>(
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/lock?breakLock=true",
                "",
                "PUT");
        }

        /// <summary>
        /// Release our lock for a title
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <returns></returns>
        public Task<bool> ReleaseLock()
        {
            // DELETE
            return SignAndRequest<bool>(
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/lock?breakLock=true",
                "",
                "DELETE");
        }

        /// <summary>
        /// Retreives the list of blob data belonging to title
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <returns></returns>
        public Task<TitleStorageBlobMetadataResult> GetBlobMetadata()
        {
            return SignAndRequest<TitleStorageBlobMetadataResult>(
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}", "");
        }

        /// <summary>
        /// Retreives the direct atoms belonging to a target blob filename
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Task<TitleStorageAtomMetadataResult> GetBlobAtoms(string filename)
        {
            return SignAndRequest<TitleStorageAtomMetadataResult>(
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/{filename}",
                "");
        }

        /// <summary>
        /// Download specified atom data
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <param name="atom"></param>
        /// <returns></returns>
        public async Task<byte[]> DownloadAtomAsync(string atom)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/{atom}");
            request.Headers.Add(HttpHeaders);

            HttpResponseMessage responseMessage = await HttpClient.SendAsync(request);
            return await responseMessage.Content.ReadAsByteArrayAsync();
        }

        /// <summary>
        /// Delete specified atom data
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <param name="atom"></param>
        /// <returns></returns>
        public async Task DeleteAtomAsync(string atom)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/{atom}");
            request.Headers.Add(HttpHeaders);

            HttpResponseMessage responseMessage = await HttpClient.SendAsync(request);
        }

        /// <summary>
        /// Fetch the blob storage uri for performing operations (PUT, POST, PATCH)
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <param name="atom"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<TitleStorageBlobUpdate> GetBlobUri(string atom, int size)
        {
            string requestBody = "{size: " + size + "}";

            return SignAndRequest<TitleStorageBlobUpdate>(
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/atoms/{atom}",
                new StringContent(requestBody),
                "");
        }

        /// <summary>
        /// Allocates a newly created atom for a specified title
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <param name="atom"></param>
        /// <param name="blockIds"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> CommitAtom(string atom, string[] blockIds, int size)
        {
            return SignAndRequest<HttpResponseMessage>(
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/atoms/{atom}?commit=true",
                new TitleStorageAtom
                {
                    BlockIds = blockIds,
                    Size = size
                }, "");
        }

        /// <summary>
        /// Update a modified blob's atom reference
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigId"></param>
        /// <param name="pfn"></param>
        /// <param name="blobAtom"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public Task<TitleStorageQuota> UpdateBlob(TitleStorageBlobAtom[] blobAtom, string displayName)
        {
            displayName = displayName.Split(',')[0].Trim();
            string clientFileTime = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");

            return SignAndRequest<TitleStorageQuota>(
                $"connectedstorage/users/xuid({Config.UserOptions.XboxUserId})/scids/{ServiceConfigurationId}/savedgames/{displayName}?clientFileTime={clientFileTime}&displayName={displayName}",
                new TitleStorageBlobAtomUpdate(new List<TitleStorageBlobAtom>(blobAtom)), "");
        }

        /// <summary>
        /// Upload provided data to blob storage
        /// </summary>
        /// <param name="xboxUserId"></param>
        /// <param name="serviceConfigurationId"></param>
        /// <param name="pfn"></param>
        /// <param name="blobMetadata"></param>
        /// <param name="blobBuffer"></param>
        /// <param name="atomUuid"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> UploadBlobAsync(TitleStorageBlobMetadata blobMetadata, byte[] blobBuffer, string atomUuid)
        {
            if (blobMetadata == null && blobMetadata == null)
                return null;

            var blobDetails = await GetBlobUri(atomUuid, blobBuffer.Length);
            if (blobDetails.BlobUri == string.Empty)
                return null;

            int blockId = 0;
            List<string> blockList = new List<string>();

            int index = 0;
            int currentBlockSize = MaxBlockUploadSize;

            while (currentBlockSize == MaxBlockUploadSize)
            {
                if (index + currentBlockSize > blobBuffer.Length)
                    currentBlockSize = blobBuffer.Length - index;

                byte[] chunk = new byte[currentBlockSize];

                Array.Copy(blobBuffer, index, chunk, 0, currentBlockSize);

                var base64BlockId = Convert.ToBase64String(BitConverter.GetBytes(blockId));
                blockList.Add(base64BlockId);
                var blockUriParam = $"comp=block&blockId={base64BlockId}&";
                var blobUri = blobDetails.BlobUri.Insert(blobDetails.BlobUri.IndexOf('?') + 1, blockUriParam);

                var request = new HttpRequestMessage(HttpMethod.Put, blobUri);
                request.Headers.Add("Connection", "Keep-Alive");
                request.Headers.Add("x-ms-blob-type", "BlockBlob");
                request.Content = new ByteArrayContent(chunk);

                var uploadResponse = await HttpClient.SendAsync(request);
                if (!uploadResponse.IsSuccessStatusCode)
                    return null;

                index += currentBlockSize;
                blockId++;
            }

            var commitResponse = await CommitAtom(atomUuid, blockList.ToArray(), blobBuffer.Length);

            return commitResponse;
        }
    }
}
