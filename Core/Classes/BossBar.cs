using System;
using SlimeCore.Core.Chat;
using SlimeCore.Enums;
using SlimeCore.Network;
using SlimeCore.Tools;

namespace SlimeCore.Core.Classes
{
	public class BossBar
	{
		public Guid UUID { get; set; }
		public BossBarActions Action { get; set; }

		public string Title = string.Empty;
		public float Health;
		public int Color;
		public int Division;
		public byte Flags;

		public int _dividers;

		public BossBar CreateUUID()
		{
			this.UUID = Guid.NewGuid();
			
			return this;
		}

		public BossBar CreateAddAction(string title, float health, int color, int division, byte flags)
		{
			ChatMessage chat_title = new ChatMessage();
			chat_title.SetText(title);

			this.Action = BossBarActions.ADD;

			this.Title = chat_title.GetJson(false);

			this.Health = health;
			this.Color = color;
			this.Division = division;
			this.Flags = flags;

			return this;
		}

		public BossBar CreateRemoveAction()
		{
			this.Action = BossBarActions.REMOVE;

			return this;
		}

		public BossBar CreateUpdateHealthAction(float health)
		{
			this.Action = BossBarActions.UPDATE_HEALTH;
			this.Health = health;

			return this;
		}

		public byte[] GetBytes()
		{
			BufferManager bm = new BufferManager();
            bm.WriteUUID(this.UUID);
            bm.WriteVarInt((int)this.Action);

            switch (this.Action)
			{
				case BossBarActions.ADD:
					bm.WriteString(this.Title);
					bm.WriteFloat(this.Health);
					bm.WriteVarInt(this.Color);
					bm.WriteVarInt(this.Division);
					bm.WriteByte(this.Flags);

					return bm.GetBytes();
				case BossBarActions.REMOVE:
					return bm.GetBytes();
				case BossBarActions.UPDATE_HEALTH:
					bm.WriteFloat(this.Health);

					return bm.GetBytes();
				default:
					Logger.Warn($"Bossbar action is uninplemented.");
					break;
			}

			return bm.GetBytes();
		}
	}
}

