﻿using UserProfileAPI.Models;

namespace UserProfileAPI.Services.Interfaces
{
    public interface IPublisherService
    {
        public void DeleteUser(string userID);
    }
}