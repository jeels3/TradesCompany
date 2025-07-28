using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradesCompany.Application.Interfaces;
using TradesCompany.Application.Services;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly IRepository<Channel> _channelRepository;
        private readonly IRepository<ApplicationUser> _userGRepository;
        private readonly IChatService _chatService;
        public ChatController(IChatRepository chatRepository,
            IRepository<Channel> channelRepository,
            IChatService chatService,
            IRepository<ApplicationUser> userGRepository
            )
        {
            _chatRepository = chatRepository;
            _channelRepository = channelRepository;
            _chatService = chatService;
            _userGRepository = userGRepository;
        }

        private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public async Task<IActionResult> UserListing()
        {
            try
            {
                var users = await _chatRepository.GetAllUserListing(userId);
                if(users == null)
                {
                    return View(new List<ApplicationUser>());
                }
                return View(users);
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
                        TempData["ChannelName"] = channelName1;
                        TempData["ReceiverName"] = receiver.UserName;
                        return View(chatdata);
                    }
                    else
                    {
                        var chatdata = await _chatRepository.GetChatMessageByChannelName(channelName2);
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

        public async Task<IActionResult> SendMessage([FromForm] ChannelMessage message)
        {
            try
            {
                await _chatService.SendMessage(message.ChannelName , message.Message , userId);
                return Ok(new {message = "Send Succesfully"});
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
