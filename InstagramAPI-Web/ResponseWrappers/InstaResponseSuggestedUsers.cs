﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstagramApi.ResponseWrappers
{
    public class InstaResponseSuggestedUsers
    {
        [JsonProperty("nodes")]
        public IList<InstaResponseUser> Users { get; set; }
    }
}