using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;

namespace FRTools.Core.Services.Interfaces
{
    public interface ITumblrService
    {
        Task<PostCreationInfo> CreatePost(string blogName, PostData postData);
    }
}
