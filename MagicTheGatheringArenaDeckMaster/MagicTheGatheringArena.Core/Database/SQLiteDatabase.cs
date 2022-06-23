using MagicTheGatheringArena.Core.Database.Models;
using MagicTheGatheringArena.Core.Services;
using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics;

namespace MagicTheGatheringArena.Core.Database
{
    public class SQLiteDatabase
    {
        #region Fields

        private SqliteConnection connection;
        private LoggerService logger;

        #endregion

        #region Properties

        public string ConnectionString { get; set; }

        #endregion

        #region Methods

        public bool EnsureDatabase(LoggerService loggerService)
        {
            logger = loggerService;

            if (connection == null)
            {
                connection = new SqliteConnection(ConnectionString);
            }

            try
            {
                connection.Open();

                // create the tables
                string tableCreateStatement = "CREATE TABLE IF NOT EXISTS 'Decks'" +
                    "(" +
                    "'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                    "'Name' TEXT NOT NULL," +
                    "'GameType' TEXT NOT NULL" +
                    ")";

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = tableCreateStatement;
                command.ExecuteNonQuery();
                command.Dispose();

                tableCreateStatement = "CREATE TABLE IF NOT EXISTS 'CardsPerDeck'" +
                    "(" +
                    "'Id' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                    "'DeckId' INTEGER NOT NULL," +
                    "'Name' TEXT NOT NULL," +
                    "'Count' INTEGER NOT NULL," +
                    "'SetSymbol' TEXT NOT NULL," +
                    "'Number' INTEGER NOT NULL," +
                    "CONSTRAINT fk_CardsPerDeck_Decks FOREIGN KEY('DeckId') REFERENCES 'Decks'('Id') ON DELETE CASCADE" +
                    ")";

                command = connection.CreateCommand();
                command.CommandText = tableCreateStatement;
                command.ExecuteNonQuery();
                command.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred attempting to ensure the database existed.{Environment.NewLine}{ex}");
                logger.Error($"An error occurred attempting to ensure the database existed.{Environment.NewLine}{ex}");

                return false;
            }
        }

        public bool SaveDeck(Deck deck)
        {
            return true;
        }

        #endregion
    }
}
