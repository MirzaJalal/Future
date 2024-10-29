using Bangla.Services.RewardAPI.Data;
using Bangla.Services.RewardAPI.Message;
using Bangla.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.RewardAPI.Services
{
    public class RewardsService : IRewardService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;

        public RewardsService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<bool> UpdateRewards(RewardMessage rewardMessage)
        {
            try
            {
                Rewards rewards = new () 
                { 
                    OrderId = rewardMessage.OrderId,
                    RewardActivity = rewardMessage.RewardActivity,
                    UserId = rewardMessage.UserId,
                    RewardIssuedDate = DateTime.Now,
                };

                await using var _db = new ApplicationDbContext(_dbOptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
