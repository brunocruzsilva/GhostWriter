﻿using Newtonsoft.Json;

namespace InstagramApi.ResponseWrappers
{
    public class InstaResponseUser
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("profile_pic_url")]
        public string ProfilePicture { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("external_url")]
        public string ExternalUrl { get; set; }

        [JsonProperty("is_verified")]
        public string IsVerified { get; set; }

        [JsonProperty("followed_by")]
        public FollowedBy FollowedBy { get; set; }
    }
}