using Bangla.Services.RewardAPI.Message;

namespace Bangla.Services.RewardAPI.Services
{
    public interface IRewardService
    {
        Task<bool> UpdateRewards(RewardMessage rewardMessage);
    }
}
