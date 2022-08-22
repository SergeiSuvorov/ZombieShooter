using Photon.Realtime;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Controller
{
    public class ResultTableController: BaseController
    {
        private Dictionary<Player, ResultTableRecord> _resultTable = new Dictionary<Player, ResultTableRecord>();
        private ResultTableUIController _resultTableUIController;
        private Transform _placeForUi;
        public ResultTableController(Transform placeForUi)
        {
            _placeForUi = placeForUi;
        }

        public void AddRecord(Player player)
        {
            if (!_resultTable.TryGetValue(player, out var record))
            {
                var newRecord = new ResultTableRecord();
                newRecord.Player = player;
                newRecord.PlayerNickName = player.NickName;
                _resultTable.Add(player, newRecord);
            }
            else
            {
                record.Player = player;
                record.KillPlayerCount = 0;
                record.KillZombieCount = 0;
                record.PlayerNickName = player.NickName;
            }
        }

        public void ChangeRecordZombieKill(Player player, int killZombieCount)
        {

            if( _resultTable.TryGetValue(player, out var record))
            {
                record.KillZombieCount += killZombieCount;
            }
            else
            {
                var newRecord = new ResultTableRecord();
                newRecord.Player = player;
                newRecord.PlayerNickName = player.NickName;
                newRecord.KillZombieCount = killZombieCount;

                _resultTable.Add(player, newRecord);
            }
        }
        public void ChangeRecordPlayerKill(Player player, int killPlayerCount)
        {
            if (_resultTable.TryGetValue(player, out var record))
            {
                record.KillPlayerCount += killPlayerCount;
            }
            else
            {
                var newRecord = new ResultTableRecord();
                newRecord.Player = player;
                newRecord.PlayerNickName = player.NickName;
                newRecord.KillPlayerCount = killPlayerCount;

                _resultTable.Add(player, newRecord);
            }
        }
        public ResultTableRecord GetRecord(Player player)
        {
            _resultTable.TryGetValue(player, out var record);
            
            if(record != null)
                ScoringPoints(record);

            return record;
        }

        public void CreateTableController()
        {
            if (_resultTableUIController == null)
            {
                _resultTableUIController = new ResultTableUIController(_placeForUi);
                AddController(_resultTableUIController);
            }
            CreateResultTable();
        }
        private void CreateResultTable()
        {
            if (_resultTableUIController == null)
                return;
            var text =string.Empty;
            var keys = _resultTable.Keys;
            foreach(var key in keys)
            {
                if (key!= null && _resultTable.TryGetValue(key, out var record))
                {
                    ScoringPoints(record);
                    
                    text += $"{record.PlayerNickName}               {record.KillZombieCount}              {record.KillPlayerCount}             {record.ScorePointResult} \n\n";
                }
            }
            _resultTableUIController.ShowTable(text);
        }

        private void ScoringPoints(ResultTableRecord record)
        {
            if(record.KillPlayerCount==0)
            {
                record.ScorePointResult = record.KillZombieCount * 100;
            }
            else 
            {
                record.ScorePointResult = 0;
            }
            
        }
    }
}



