﻿namespace Wallone.Auth.Domain.Users
{
    public sealed class Permission
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Permission(
            int id,
            string name)
        {
            Id = id;
            Name = name;
        }
    }
}