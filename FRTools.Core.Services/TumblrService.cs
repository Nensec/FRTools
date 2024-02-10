using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using FRTools.Core.Data.DataModels.NewsReaderModels;
using FRTools.Core.Services.Interfaces;

namespace FRTools.Core.Services
{
    public class TumblrService : ITumblrService
    {
        private TumblrClient GetClient()
        {
            return new TumblrClientFactory().Create<TumblrClient>(
                Environment.GetEnvironmentVariable("TumblrClientId"),
                Environment.GetEnvironmentVariable("TumblrSecret"),
                new Token(
                Environment.GetEnvironmentVariable("TumblrOAuthToken"),
                Environment.GetEnvironmentVariable("TumblrOAuthSecret")));
        }

        public async Task<PostCreationInfo> CreatePost(string blogName, PostData postData)
        {
            var postParamaters = (MethodParameterSet)typeof(PostData).GetField("parameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(postData)!;
            postParamaters.Add("native_inline_images", true);
            using (var client = GetClient())
            {
                return await client.CreatePostAsync("frtools", postData);
            }
        }
    }
}
