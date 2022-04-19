﻿namespace Assets.CoreData.Interfaces
{
    public interface IWithId
    {
        string Id { get; set; }
    }

    public interface IWithGettableId
    {
        string Id { get; }
    }

    public interface IWithCalculatedId
    {
        string GetId(string ParentId);
    }
}