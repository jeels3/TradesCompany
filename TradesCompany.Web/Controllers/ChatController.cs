using DocumentFormat.OpenXml.Spreadsheet;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;
using TradesCompany.Web.ViewModel;
using Channel = TradesCompany.Domain.Entities.Channel;

namespace TradesCompany.Web.Controllers
{
    [Authorize(Roles = "USER , ADMIN , EMPLOYEE")]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly IRepository<Channel> _channelRepository;
        private readonly IRepository<ApplicationUser> _userGRepository;
        private readonly IChatService _chatService;
        private readonly IRepository<ChannelUser> _channelUserGRepository;
        public ChatController(IChatRepository chatRepository,
            IRepository<Channel> channelRepository,
            IChatService chatService,
            IRepository<ApplicationUser> userGRepository,
            IRepository<ChannelUser> channelUserGRepository
            )
        {
            _chatRepository = chatRepository;
            _channelRepository = channelRepository;
            _chatService = chatService;
            _userGRepository = userGRepository;
            _channelUserGRepository = channelUserGRepository;
        }

        private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public async Task<IActionResult> UserListing()
        {
            try
            {
                var data = await _chatRepository.GetUserAndGroupListingWithCount(userId);

                var users = await _chatRepository.GetAllUserListing(userId);
                var groups = await _chatRepository.GetGroupByUserId(userId);
                if (users == null && groups == null)
                {
                    UserAndGroupListing nullmodel = new UserAndGroupListing
                    {
                        Users = new List<ApplicationUser>(),
                        channels = new List<Channel>()
                    };
                    return View(nullmodel);
                }
                UserAndGroupListing model = new UserAndGroupListing
                {
                    Users = users,
                    channels = groups
                };
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Something Went Wrong";
                return View();
            }
        }

        public async Task<IActionResult> OneToOneChat(string receiverId)
        {
            var receiver = await _userGRepository.GetByIdAsync(receiverId);
            // Create ChannelId 
            string channelName1 = $"P_{userId}_{receiverId}";
            string channelName2 = $"P_{receiverId}_{userId}";
            // If Exist Than Fatch Chat Data Else Insert ChannelId
            try
            {
                var result1 = await _chatRepository.CheckChennelNameIsExists(channelName1);
                var result2 = await _chatRepository.CheckChennelNameIsExists(channelName2);
                if (result1 || result2)
                {
                    // Fatch Chat 
                    if (result1)
                    {
                        var chatdata = await _chatRepository.GetChatMessageByChannelName(channelName1);
                        if (chatdata == null)
                        {
                            return View(new List<ChannelMessage>());
                        }
                        TempData["ChannelName"] = channelName1;
                        TempData["ReceiverName"] = receiver.UserName;
                        return View(chatdata);
                    }
                    else
                    {
                        var chatdata = await _chatRepository.GetChatMessageByChannelName(channelName2);
                        if (chatdata == null)
                        {
                            return View(new List<ChannelMessage>());
                        }
                        TempData["ChannelName"] = channelName2;
                        TempData["ReceiverName"] = receiver.UserName;
                        return View(chatdata);
                    }
                }
                else
                {
                    // Create The Channel
                    Channel channel = new Channel
                    {
                        ChannelName = channelName1,
                        CreatorId = userId,
                    };
                    await _channelRepository.InsertAsync(channel);
                    await _channelRepository.SaveAsync();

                    ChannelUser channelUser = new ChannelUser
                    {
                        ChannelId = channel.Id,
                        UserId = userId
                    };

                    ChannelUser channelUser2 = new ChannelUser
                    {
                        ChannelId = channel.Id,
                        UserId = receiver.Id
                    };

                    await _channelUserGRepository.InsertAsync(channelUser);
                    await _channelUserGRepository.InsertAsync(channelUser2);
                    await _channelUserGRepository.SaveAsync();



                    TempData["ChannelName"] = channel.ChannelName;
                    TempData["ReceiverName"] = receiver.UserName;

                    return View(new List<ChannelMessage>());
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] Dictionary<string, object> data)
        {
            try
            {
                var ChannelName = data["ChannelName"].ToString();
                var GroupIds = ((JsonElement)data["GroupIds"]).EnumerateArray().Select(x => x.GetString()).ToArray();
                if (string.IsNullOrEmpty(ChannelName) || GroupIds == null || GroupIds.Length <= 1)
                {
                    TempData["errorMessage"] = "Channel Name or Group Ids are not valid.";
                    return BadRequest();
                }
                // Check Channel Name is Already Exists
                var isExists = await _chatRepository.CheckChennelNameIsExists(ChannelName);
                if (isExists)
                {
                    TempData["errorMessage"] = "Channel Name Already Exists.";
                    return BadRequest();
                }
                // Create Channel
                Channel channel = new Channel
                {
                    ChannelName = $"Group_{ChannelName}",
                    CreatorId = userId,
                };
                await _channelRepository.InsertAsync(channel);
                await _channelRepository.SaveAsync();

                foreach (var ids in GroupIds)
                {
                    ChannelUser channelUser = new ChannelUser
                    {
                        ChannelId = channel.Id,
                        UserId = ids
                    };
                    await _channelUserGRepository.InsertAsync(channelUser);
                }
                ChannelUser channelUser2 = new ChannelUser
                {
                    ChannelId = channel.Id,
                    UserId = userId
                };
                await _channelUserGRepository.InsertAsync(channelUser2);

                await _channelUserGRepository.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Something Went Wrong : {ex.Message}";
                return Ok();
            }
        }

        //public async Task<IActionResult> GroupListing()
        //{
        //    try
        //    {
        //        // Get All Channel By UserId
        //        var channels = await _channelRepository.GetAllAsync(x => x.ChannelUsers.Any(cu => cu.UserId == userId));
        //        if (channels == null || !channels.Any())
        //        {
        //            return View(new List<Channel>());
        //        }
        //        return View(channels);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["errorMessage"] = "Something Went Wrong";
        //        return View(new List<Channel>());
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GroupChat(string channelName)
        {
            try
            {
                if (string.IsNullOrEmpty(channelName))
                {
                    TempData["errorMessage"] = "Channel Name is not valid.";
                    return RedirectToAction("UserListing", "Chat");
                }
                // Get Channel Id By Channel Name
                var channelId = await _chatRepository.GetChannelIdByChannelName(channelName);
                if (channelId == 0)
                {
                    TempData["errorMessage"] = "Channel Not Found.";
                    return RedirectToAction("UserListing", "Chat");
                }
                // Check User is Exist in Channel
                var UserInChennel = await _chatRepository.CheckUserInChannel(channelId, userId);
                if(!UserInChennel)
                {
                    TempData["errorMessage"] = "You are not a member of this channel.";
                    return RedirectToAction("UserListing", "Chat");
                }
                // Get Chat Data By Channel Id
                var chatdata = await _chatRepository.GetChatMessageByChannelName(channelName);
                if (chatdata == null)
                {
                    return View(new List<ChannelMessage>());
                }
                TempData["ChannelName"] = channelName;
                TempData["GroupName"] = channelName;
                return View(chatdata);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Something Went Wrong : {ex.Message}";
                return View(new List<ChannelMessage>());
            }
        }
    }
}
