﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Boolean.Network.Transmission;
using Boolean.Network.Messages.Storage.Composers;

namespace Boolean.Habbo.Characters
{
    class Character
    {
        public int Id;
        public string Username;
        public int Rank;

        public int Credits;
        public int Pixels;
        public int Soundvolume;

        public Character(DataRow Row)
        {
            this.Id = (int)Row["id"];
            this.Username = (string)Row["username"];
            this.Rank = (int)Row["rank"];
            this.Credits = (int)Row["credits"];
            this.Pixels = (int)Row["pixels"];
            this.Soundvolume = (int)Row["sound_volume"];
        }

        public int GetAchievementScore()
        {
            var Result = 0;

            foreach (var AchievementProgress in StorageHandler.GetCharacterAchievements(Id))
            {
                var Achievement = AchievementHandler.GetAchievement(AchievementProgress.Id);

                Result += (AchievementProgress.CurrentLevel * Achievement.ScorePerLevel);
            }

            return Result;
        }

        public void RefreshAchievement(int AchievementId)
        {
            var Achievement = AchievementHandler.GetAchievement(AchievementId);

            if (Achievement == null)
            {
                return;
            }

            if (Rank < Achievement.RankRequired)
            {
                return;
            }

            MessageHandler.HandleComposer(GetSession(), new AchievementComposer(), Achievement, this);
        }

        public Session GetSession()
        {
            return SessionHandler.GetSessionByCharacterId(Id);
        }
    }
}
