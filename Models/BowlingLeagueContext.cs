using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BowlingLeague.Models
{
    public partial class BowlingLeagueContext : DbContext
    {
        public BowlingLeagueContext()
        {
        }

        public BowlingLeagueContext(DbContextOptions<BowlingLeagueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BowlerScore> BowlerScores { get; set; }
        public virtual DbSet<Bowler> Bowlers { get; set; }
        public virtual DbSet<MatchGame> MatchGames { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }
        public virtual DbSet<TourneyMatch> TourneyMatches { get; set; }
        public virtual DbSet<ZtblBowlerRating> ZtblBowlerRatings { get; set; }
        public virtual DbSet<ZtblSkipLabel> ZtblSkipLabels { get; set; }
        public virtual DbSet<ZtblWeek> ZtblWeeks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source = BowlingLeague.sqlite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BowlerScore>(entity =>
            {
                entity.HasKey(e => new { e.MatchId, e.GameNumber, e.BowlerId });

                entity.ToTable("Bowler_Scores");

                entity.HasIndex(e => e.BowlerId)
                    .HasName("BowlerID");

                entity.HasIndex(e => new { e.MatchId, e.GameNumber })
                    .HasName("MatchGamesBowlerScores");

                entity.Property(e => e.MatchId)
                    .HasColumnName("MatchID")
                    .HasColumnType("int");

                entity.Property(e => e.GameNumber).HasColumnType("smallint");

                entity.Property(e => e.BowlerId)
                    .HasColumnName("BowlerID")
                    .HasColumnType("int");

                entity.Property(e => e.HandiCapScore)
                    .HasColumnType("smallint")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RawScore)
                    .HasColumnType("smallint")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WonGame)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Bowler)
                    .WithMany(p => p.BowlerScores)
                    .HasForeignKey(d => d.BowlerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Bowler>(entity =>
            {
                entity.HasKey(e => e.BowlerId);

                entity.HasIndex(e => e.BowlerLastName)
                    .HasName("BowlerLastName");

                entity.HasIndex(e => e.TeamId)
                    .HasName("BowlersTeamID");

                entity.Property(e => e.BowlerId)
                    .HasColumnName("BowlerID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.BowlerAddress).HasColumnType("nvarchar (50)");

                entity.Property(e => e.BowlerCity).HasColumnType("nvarchar (50)");

                entity.Property(e => e.BowlerFirstName).HasColumnType("nvarchar (50)");

                entity.Property(e => e.BowlerLastName).HasColumnType("nvarchar (50)");

                entity.Property(e => e.BowlerMiddleInit).HasColumnType("nvarchar (1)");

                entity.Property(e => e.BowlerPhoneNumber).HasColumnType("nvarchar (14)");

                entity.Property(e => e.BowlerState).HasColumnType("nvarchar (2)");

                entity.Property(e => e.BowlerZip).HasColumnType("nvarchar (10)");

                entity.Property(e => e.TeamId)
                    .HasColumnName("TeamID")
                    .HasColumnType("int");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Bowlers)
                    .HasForeignKey(d => d.TeamId);
            });

            modelBuilder.Entity<MatchGame>(entity =>
            {
                entity.HasKey(e => new { e.MatchId, e.GameNumber });

                entity.ToTable("Match_Games");

                entity.HasIndex(e => e.MatchId)
                    .HasName("TourneyMatchMatchGames");

                entity.HasIndex(e => e.WinningTeamId)
                    .HasName("Team1ID");

                entity.Property(e => e.MatchId)
                    .HasColumnName("MatchID")
                    .HasColumnType("int");

                entity.Property(e => e.GameNumber).HasColumnType("smallint");

                entity.Property(e => e.WinningTeamId)
                    .HasColumnName("WinningTeamID")
                    .HasColumnType("int")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.MatchGames)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.HasIndex(e => e.TeamId)
                    .HasName("TeamID")
                    .IsUnique();

                entity.Property(e => e.TeamId)
                    .HasColumnName("TeamID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.CaptainId)
                    .HasColumnName("CaptainID")
                    .HasColumnType("int");

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasColumnType("nvarchar (50)");
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(e => e.TourneyId);

                entity.Property(e => e.TourneyId)
                    .HasColumnName("TourneyID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.TourneyDate).HasColumnType("date");

                entity.Property(e => e.TourneyLocation).HasColumnType("nvarchar (50)");
            });

            modelBuilder.Entity<TourneyMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId);

                entity.ToTable("Tourney_Match");

                entity.HasIndex(e => e.EvenLaneTeamId)
                    .HasName("Tourney_MatchEven");

                entity.HasIndex(e => e.OddLaneTeamId)
                    .HasName("TourneyMatchOdd");

                entity.HasIndex(e => e.TourneyId)
                    .HasName("TourneyMatchTourneyID");

                entity.Property(e => e.MatchId)
                    .HasColumnName("MatchID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.EvenLaneTeamId)
                    .HasColumnName("EvenLaneTeamID")
                    .HasColumnType("int")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Lanes).HasColumnType("nvarchar (5)");

                entity.Property(e => e.OddLaneTeamId)
                    .HasColumnName("OddLaneTeamID")
                    .HasColumnType("int")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TourneyId)
                    .HasColumnName("TourneyID")
                    .HasColumnType("int")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.EvenLaneTeam)
                    .WithMany(p => p.TourneyMatchEvenLaneTeams)
                    .HasForeignKey(d => d.EvenLaneTeamId);

                entity.HasOne(d => d.OddLaneTeam)
                    .WithMany(p => p.TourneyMatchOddLaneTeams)
                    .HasForeignKey(d => d.OddLaneTeamId);

                entity.HasOne(d => d.Tourney)
                    .WithMany(p => p.TourneyMatches)
                    .HasForeignKey(d => d.TourneyId);
            });

            modelBuilder.Entity<ZtblBowlerRating>(entity =>
            {
                entity.HasKey(e => e.BowlerRating);

                entity.ToTable("ztblBowlerRating");

                entity.Property(e => e.BowlerRating).HasColumnType("nvarchar (15)");

                entity.Property(e => e.BowlerHighAvg).HasColumnType("smallint");

                entity.Property(e => e.BowlerLowAvg).HasColumnType("smallint");
            });

            modelBuilder.Entity<ZtblSkipLabel>(entity =>
            {
                entity.HasKey(e => e.LabelCount);

                entity.ToTable("ztblSkipLabels");

                entity.Property(e => e.LabelCount)
                    .HasColumnType("int")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<ZtblWeek>(entity =>
            {
                entity.HasKey(e => e.WeekStart);

                entity.ToTable("ztblWeeks");

                entity.Property(e => e.WeekStart).HasColumnType("date");

                entity.Property(e => e.WeekEnd).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}