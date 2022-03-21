﻿using System.Threading.Tasks;

namespace Core.Utilities.Mail
{
    public interface IMailService
    {
        Task Send(EmailMessage emailMessage);
    }
}