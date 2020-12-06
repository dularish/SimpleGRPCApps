using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleGRPCService.Services
{
    public class CricketScoreUpdateService : CricketScoreUpdater.CricketScoreUpdaterBase
    {
        private static TotalScoreBoard _scoreBoard = new TotalScoreBoard() { Runs = 0, TotalBallsBowled = 0, Wickets = 0 };
        public override Task<TotalScoreBoard> UpdateBallInfo(BallUpdateRequest request, ServerCallContext context)
        {
            _scoreBoard.Runs += request.RunsOffBall;
            _scoreBoard.Wickets += request.Wickets;
            _scoreBoard.TotalBallsBowled++;

            return Task.FromResult<TotalScoreBoard>(_scoreBoard) ;
        }
    }
}
