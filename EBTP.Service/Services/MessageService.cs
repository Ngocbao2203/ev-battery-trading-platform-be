using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Repository.IRepositories;
using EBTP.Service.Abstractions.Shared;
using EBTP.Service.DTOs.ChatThread;
using EBTP.Service.DTOs.Message;
using EBTP.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMessageNotifier _messageNotifier;

        public MessageService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IMessageRepository messageRepository,
            ICloudinaryService cloudinaryService,
            IMessageNotifier messageNotifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageRepository = messageRepository;
            _cloudinaryService = cloudinaryService;
            _messageNotifier = messageNotifier;
        }

        public async Task<Result<bool>> SoftDeleteChatThreadAsync(Guid chatThreadId)
        {
            var chatThread = await _unitOfWork.chatThreadRepository
                .GetChatThreadByIdAsync(chatThreadId);

            if (chatThread == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Didn't find any chat thread, please try again!",
                    Data = false
                };
            }

            _unitOfWork.chatThreadRepository.SoftRemove(chatThread);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Remove chat thread successfully" : "Remove chat thread fail",
                Data = result > 0
            };
        }

        public async Task<Result<bool>> SoftDeleteMessageAsync(Guid messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);

            if (message == null)
            {
                return new Result<bool>
                {
                    Error = 1,
                    Message = "Didn't find any message, please try again!",
                    Data = false
                };
            }

            _messageRepository.SoftRemove(message);

            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Remove message successfully" : "Remove message fail",
                Data = result > 0
            };
        }

        public async Task<Result<List<ViewChatThreadDTO>>> GetChatThreadsByUserIdAsync(Guid userId)
        {
            var result = _mapper.Map<List<ViewChatThreadDTO>>(
                await _unitOfWork.chatThreadRepository.GetChatThreadByUserIdAsync(userId));

            return new Result<List<ViewChatThreadDTO>>
            {
                Error = 0,
                Message = "View online chat thread successfully",
                Data = result
            };
        }

        public async Task<Result<ViewChatThreadDTO>> GetChatThreadByIdAsync(Guid chatThreadId)
        {
            var result = _mapper.Map<ViewChatThreadDTO>(
                await _unitOfWork.chatThreadRepository.GetChatThreadByIdAsync(chatThreadId));

            return new Result<ViewChatThreadDTO>
            {
                Error = 0,
                Message = "View online chat thread successfully",
                Data = result
            };
        }
        public async Task<Result<ViewChatThreadDTO>> GetChatThreadByListingIdAsync(Guid listingId)
        {
            var result = _mapper.Map<ViewChatThreadDTO>(
                await _unitOfWork.chatThreadRepository.GetChatThreadByListingAsync(listingId));

            return new Result<ViewChatThreadDTO>
            {
                Error = 0,
                Message = "View online chat thread successfully",
                Data = result
            };
        }
        public async Task<Result<object>> SendMessageAsync(SendMessageDTO sendMessage)
        {
            var chatThread = await _unitOfWork.chatThreadRepository
                .GetChatThreadByIdAsync(sendMessage.ChatThreadId);

            if (chatThread == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any chat thread, please try again!",
                    Data = false
                };
            }

            var user = await _unitOfWork.userRepository.GetByIdAsync(sendMessage.SenderId);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any user, please try again!",
                    Data = null
                };
            }

            var message = new Message
            {
                ChatThreadId = chatThread.Id,
                SenderId = user.Id,
                MessageText = sendMessage.MessageText
            };

            await _messageRepository.AddAsync(message);

            var result = await _unitOfWork.SaveChangeAsync();

            if (result > 0)
            {

                _messageRepository.Update(message);

                await _unitOfWork.SaveChangeAsync();
            }
            if (result > 0)
            {
                var responseData = new
                {
                    chatThreadId = message.ChatThreadId,
                    message.CreatedBy,
                    message.CreationDate,
                    message.SenderId,
                    message.Id,
                    message.MessageText,
                    message.IsRead
                };

                await _messageNotifier.NotifyMessageSentAsync(chatThread.Id, chatThread.UserId, chatThread.ParticipantId, responseData, "Message");
                return new Result<object>
                {
                    Error = 0,
                    Message = "Message sent successfully!",
                    Data = responseData
                };
            }
            else
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Failed to send message.",
                    Data = null
                };
            }
        }

        public async Task<Result<object>> StartThreadAsync(CreateChatThreadDTO chatThread)
        {
            var user = await _unitOfWork.userRepository.GetByIdAsync(chatThread.UserId);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any user, please try again!",
                    Data = null
                };
            }

            var consultant = await _unitOfWork.userRepository.GetByIdAsync(chatThread.ParticipantId);

            if (consultant == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Didn't find any user, please try again!",
                    Data = null
                };
            }
            var listing = await _unitOfWork.listingRepository.GetByIdAsync(chatThread.ListingId);

            if (listing == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "no listing with that id was found",
                    Data = null
                };
            }

            var existingThread = await _unitOfWork.chatThreadRepository
                .GetExistingChatThreadByIdAsync(chatThread.UserId, chatThread.ParticipantId, chatThread.ListingId);

            if (existingThread != null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Chat thread already exists!",
                    Data = null
                };
            }

            var thread = _mapper.Map<ChatThread>(chatThread);

            thread.Status = "Active";

            await _unitOfWork.chatThreadRepository.AddAsync(thread);

            var result = await _unitOfWork.SaveChangeAsync();

            if (result > 0)
            {
                var responseData = new
                {
                    Id = thread.Id,
                    UserId = thread.UserId,
                    ParticipantId = thread.ParticipantId,
                    ListingId = thread.ListingId,
                    Status = thread.Status,
                };

                return new Result<object>
                {
                    Error = 0,
                    Message = "Chat thread started successfully!",
                    Data = responseData
                };
            }
            else
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Failed to start chat thread.",
                    Data = null
                };
            }
        }
    }
}
