﻿using SharedCookbook.Api.Data.Entities;

namespace SharedCookbook.Api.Repositories.Interfaces;

public interface ICookbookInvitationRepository
{
    CookbookInvitation? GetSingle(int id);
    List<CookbookInvitation> GetInvitations(int personId);
    void Add(CookbookInvitation invitation);
    void Delete(int id);
    CookbookInvitation Update(CookbookInvitation invitation);
    bool Save();
}