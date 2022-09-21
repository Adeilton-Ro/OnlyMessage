using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.DTO;
public record UserAndFriendMessages(User? User, User? Friend);