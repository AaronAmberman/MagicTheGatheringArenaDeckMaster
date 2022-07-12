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

        public bool IsDeckNameUnique(string name, int deckId)
        {
            try
            {
                SqliteCommand command = connection.CreateCommand();

                if (deckId == -1)
                {                    
                    command.CommandText = "SELECT Id 'Decks' WHERE 'Name' = :name";
                    command.Parameters.Add(new SqliteParameter(":name", name));
                }
                else
                {
                    command.CommandText = "SELECT Id 'Decks' WHERE 'Name' = :name AND 'Id' = :id";
                    command.Parameters.Add(new SqliteParameter(":name", name));
                    command.Parameters.Add(new SqliteParameter(":id", deckId));
                }

                long? id = (long?)command.ExecuteScalar();

                if (id.HasValue) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred attempting to check to see if the deck name was unique.{Environment.NewLine}{ex}");
                logger.Error($"An error occurred attempting to check to see if the deck name was unique.{Environment.NewLine}{ex}");

                return false;
            }
        }

        public bool SaveDeck(Deck deck)
        {
            SqliteTransaction transaction = null;

            try
            {
                connection.Open();

                transaction = connection.BeginTransaction();

                // save the deck first
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO 'Decks' VALUES(NULL, :name, :gameType)";
                command.Parameters.Add(new SqliteParameter(":name", deck.Name));
                command.Parameters.Add(new SqliteParameter(":gameType", deck.GameType));

                int result = command.ExecuteNonQuery();

                if (result == 0)
                {
                    transaction.Rollback();

                    Debug.WriteLine("Could not insert the deck into the database.");
                    logger.Error("Could not insert the deck into the database.");

                    return false;
                }

                command.Dispose();

                // get the deck id second
                command = connection.CreateCommand();
                command.CommandText = "SELECT last_insert_rowid()";

                long deckId = (long)command.ExecuteScalar();

                deck.Id = deckId;

                command.Dispose();

                // set the deck id on all the cards
                // save all the cards
                foreach (Card card in deck.Cards)
                {
                    card.DeckId = deckId;

                    command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO 'CardsPerDeck' VALUES(NULL, :deckId, :name, :count, :setSymbol, :number)";
                    command.Parameters.Add(new SqliteParameter(":deckId", deckId));
                    command.Parameters.Add(new SqliteParameter(":name", card.Name));
                    command.Parameters.Add(new SqliteParameter(":count", card.Count));
                    command.Parameters.Add(new SqliteParameter(":setSymbol", card.SetSymbol));
                    command.Parameters.Add(new SqliteParameter(":number", card.CardNumber));

                    result = command.ExecuteNonQuery();

                    if (result == 0)
                    {
                        transaction.Rollback();

                        Debug.WriteLine($"Could not insert card {card.Name} into the database.");
                        logger.Error($"Could not insert card {card.Name} into the database.");

                        return false;
                    }

                    command.Dispose();

                    // get the card id
                    command = connection.CreateCommand();
                    command.CommandText = "SELECT last_insert_rowid()";

                    long cardId = (long)command.ExecuteScalar();

                    card.Id = cardId;
                }

                transaction.Commit();

                return true;
            }
            catch (Exception ex) 
            {
                transaction?.Rollback();

                Debug.WriteLine($"An error occurred attempting to save the deck to the database.{Environment.NewLine}{ex}");
                logger.Error($"An error occurred attempting to save the deck to the database.{Environment.NewLine}{ex}");

                return false;
            }
        }

        #endregion
    }
}
