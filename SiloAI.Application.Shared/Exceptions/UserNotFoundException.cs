namespace SiloAI.Application.Shared;
public class UserNotFoundException : Exception
{
    public UserNotFoundException()
        : base("کاربر با مشخصات ارسال شده یافت نشد")
    {
    }
}
