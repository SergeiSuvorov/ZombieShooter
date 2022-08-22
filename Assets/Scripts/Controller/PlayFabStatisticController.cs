using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Controller
{
    public class PlayFabStatisticController:BaseController
    {
        public Action<StatisticValue> onGetStatistic;
        public Action<PlayFabError> onPlayFabError;
        public void GetStatistics()
        {
            PlayFabClientAPI.GetPlayerStatistics(
                new GetPlayerStatisticsRequest(),
                OnGetStatistics,
                error => OnError(error)
            );
        }

        private void OnGetStatistics(GetPlayerStatisticsResult result)
        {
            var xp = result.Statistics.Where(x => x.StatisticName == StatisticParametrNamePlayerLists.ExperiencePoints).FirstOrDefault();
            if (xp == null)
            {
                UpdateStatistics(StatisticParametrNamePlayerLists.ExperiencePoints, 0);
            }
            else
            {
                onGetStatistic?.Invoke(xp);
            }
        }

        public void UpdateStatistics(string statisticName, int statisticValue)
        {
            PlayFabClientAPI.UpdatePlayerStatistics
                (
                    new UpdatePlayerStatisticsRequest
                    {
                        Statistics = new List<StatisticUpdate> {
                        new StatisticUpdate { StatisticName = statisticName, Value =  statisticValue },}
                    },
                        result => { GetStatistics(); },
                        error => OnError(error)
                );
        }

        private void OnError(PlayFabError error)
        {
            onPlayFabError?.Invoke(error);
        }
    }
}


