﻿using Miki.Framework;
using Miki.Common;
using Miki.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Miki.Models
{
    public class LevelRole
    {
        public long GuildId { get; set; }
        public long RoleId { get; set; }

        public int RequiredLevel { get; set; }
		public bool Automatic { get; set; }
		public bool Optable { get; set; }
		public long RequiredRole { get; set; }
		public int Price { get; set; }

        [NotMapped]
        public IDiscordRole Role => new RuntimeRole(Bot.Instance.Client.GetGuild(GuildId.FromDbLong()).GetRole(RoleId.FromDbLong()));

		public static async Task<LevelRole> CreateAsync(long guildId, long roleId)
		{
			using (MikiContext context = new MikiContext())
			{
				var role = (await context.LevelRoles.AddAsync(new LevelRole()
				{
					GuildId = guildId,
					RoleId = roleId
				})).Entity;
				await context.SaveChangesAsync();
				return role;
			}
		}

		public static async Task<LevelRole> GetAsync(long guildId, long roleId)
		{
			using(MikiContext context = new MikiContext())
			{
				LevelRole role = await context.LevelRoles.FindAsync(guildId, roleId);
				if (role == null)
					role = await CreateAsync(guildId, roleId);
				return role;
			}
		}
    }
}