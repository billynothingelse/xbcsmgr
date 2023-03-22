// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace XboxCsMgr.XboxLive.TitleStorage
{
    /// <summary>
    /// Returns the amount of storage space allocated and used.
    /// </summary>
    public class TitleStorageQuota
    {
        /// <summary>
        /// Maximum number of bytes that can be used in title storage of type StorageType.
        /// Note that this is a soft limit and the used bytes may actually exceed this value.
        /// </summary>
        public ulong QuotaBytes { get; set; }

        /// <summary>
        /// Number of bytes used in title storage of type StorageType.
        /// </summary>
        public ulong UsedBytes { get; set; }

        /// <summary>
        /// The TitleStorageType associated with the quota.
        /// </summary>
        public TitleStorageType StorageType { get; private set; }

        /// <summary>
        /// The ID of the user associated with the storage area
        /// </summary>
        public string XboxUserId { get; private set; }

        public string ownerChangeId { get; set; }

        /// <summary>
        /// The service configuration ID to get the quota from
        /// </summary>
        public string ServiceConfigurationId { get; private set; }
    }
}
