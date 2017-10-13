﻿using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "Prizes.csv";
        private const string PeopleFile = "People.csv";
        private const string TeamFile = "Teams.csv";
        private const string TournamentFile = "Tournaments.csv";
        private const string MatchupFile = "Matchup.csv";
        private const string MatchupEntryFile = "MatchupEntry.csv";

        /// <summary>
        /// Saves a new prize to the textfile
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the Id</returns>
        public Prize CreatePrize(Prize model)
        {
            //load the text file and convert the text to List<Prize>
            List<Prize> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizes();

            // find the max id

            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            // add the new record with the new id (max + 1)
            model.Id = currentId;
            prizes.Add(model);

            // convert the prizes to a list<string>
            // save the list<string> to the text file
            prizes.SaveToPrizeFile(PrizesFile);

            //return the model
            return model;
        }

        public Person CreatePerson(Person model)
        {
            List<Person> people = PeopleFile.FullFilePath().LoadFile().ConvertToPerson();

            int currentId = 1;
            if(people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            people.Add(model);

            people.SaveToPeopleFile(PeopleFile);

            return model;
        }

        public List<Person> GetPerson_All()
        {
            return PeopleFile.FullFilePath().LoadFile().ConvertToPerson();
        }

        public Team CreateTeam(Team model)
        {
            List<Team> teams = TeamFile.FullFilePath().LoadFile().ConvertToTeams(PeopleFile);

            int currentId = 1;
            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            teams.Add(model);

            teams.SaveToTeamFile(TeamFile);

            return model;
        }

        public List<Team> GetTeam_All()
        {
            List<Team> teams = TeamFile.FullFilePath().LoadFile().ConvertToTeams(PeopleFile);
            return teams;
        }

        public void CreateTournament(Tournament model)
        {
            List<Tournament> tournaments = TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournaments(TeamFile, PeopleFile, PrizesFile);

            int currentId = 1;
            if(tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            model.SaveRoundsToFile(MatchupFile, MatchupEntryFile);

            tournaments.Add(model);

            tournaments.SaveToTournamentFile(TournamentFile);
        }

        public List<Tournament> GetTournament_All()
        {
            return TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournaments(TeamFile, PeopleFile, PrizesFile);
        }
    }
}
