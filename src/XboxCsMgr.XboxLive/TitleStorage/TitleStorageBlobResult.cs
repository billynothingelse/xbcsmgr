// Copyright (c) Microsoft Corporation
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace XboxCsMgr.XboxLive.TitleStorage
{
    /// <summary>
    /// Blob data returned from the cloud.
    /// </summary>
    public class TitleStorageBlobResult
    {
        /// <summary>
        /// Updated TitleStorageBlobMetadata object following an upload or download.
        /// </summary>
        public TitleStorageBlobMetadata BlobMetadata { get; set; }

        /// <summary>
        /// The contents of the title storage blob.
        /// </summary>
        public byte[] BlobBuffer { get; set; }
    }
}
