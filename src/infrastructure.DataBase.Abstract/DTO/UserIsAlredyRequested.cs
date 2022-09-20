using Domain.Entities;

namespace Infrastructure.DataBase.Abstract.DTO;
public record UserIsAlredyRequested(User User, bool IsAlredyRequested);