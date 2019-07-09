using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSystem.Combat
{
    public class CombatManager
    {
        private List<ParticipantDetails> participants = new List<ParticipantDetails>();
        private Dictionary<string, ParticipantDetails> participantLookup = new Dictionary<string, ParticipantDetails>();

        public Arena Arena { get; set; }

        public List<Party> ParticipatingParties { get; } = new List<Party>();

        public bool IsPartyActive(Party party)
        {
            if (party != null && ParticipatingParties.Contains(party))
            {
                foreach (var member in party.Members)
                {
                    if (IsParticipantActive(member.Identifier))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsParticipantActive(string characterIdentifier)
        {
            return !(FindParticipant(characterIdentifier)?.IsKnockedOut ?? true);
        }

        private void BuildParticipantListIfNeeded()
        {
            if (participants.Count == 0)
            {
                foreach (var party in ParticipatingParties)
                {
                    if (party != null)
                    {
                        foreach (var member in party.Members)
                        {
                            if (member != null)
                            {
                                var participant = new ParticipantDetails { Character = member, Party = party };
                                participants.Add(participant);
                                participantLookup.Add(member.Identifier, participant);
                            }
                        }
                    }
                }
            }
        }

        public List<ParticipantDetails> AllParticipants
        {
            get
            {
                BuildParticipantListIfNeeded();
                return participants;
            }
        }

        public ParticipantDetails FindParticipant(string characterIdentifier)
        {
            BuildParticipantListIfNeeded();
            if (!String.IsNullOrEmpty(characterIdentifier) && participantLookup.ContainsKey(characterIdentifier))
            {
                return participantLookup[characterIdentifier];
            }
            return null;
        }

        public virtual void DetermineSurprise()
        {
        }

        public virtual void SetStartingPositions()
        {
        }

        public virtual void DetermineInitiative()
        {
            foreach (var participant in AllParticipants)
            {
                participant.RollInitiative();
            }
            AllParticipants.Sort();
        }

        public virtual void RunBattle()
        {
            if (Arena == null) throw new GameException("No arena for the battle");
            if (AllParticipants.Count == 0) throw new GameException("No participants for the battle");

            DetermineSurprise();
            SetStartingPositions();
            DetermineInitiative();
            while (!IsBattleOver())
            {
                NextRound();
            }
        }

        public virtual void NextRound()
        {
            BeginRound();
            foreach (var participant in AllParticipants)
            {
                TakeTurn(participant);
                if (IsBattleOver())
                {
                    break;
                }
            }
            EndRound();
        }

        public virtual void BeginRound()
        {
        }

        public virtual void TakeTurn(ParticipantDetails participant)
        {
        }

        public virtual void EndRound()
        {
        }

        public virtual bool IsBattleOver()
        {
            int numActiveParties = 0;
            foreach (var party in ParticipatingParties)
            {
                if (IsPartyActive(party))
                {
                    numActiveParties++;
                }
            }
            // battle is over when only 1 party is left standing, or if everyone is out
            return numActiveParties <= 1; 
        }
    }

    public class ParticipantDetails : IComparable
    {
        public int Initiative { get; set; }

        public bool IsSurprised { get; set; }

        public bool IsKnockedOut { get; set; }

        public CharacterInstance Character { get; set; }

        public Party Party { get; set; }

        public void RollInitiative()
        {
            Initiative = Character?.AbilityCheck(Ability.Dexterity) ?? 0;
        }

        public int CompareTo(object obj)
        {
            ParticipantDetails otherDetails = obj as ParticipantDetails;
            if (otherDetails != null)
            {
                int result = Initiative.CompareTo(otherDetails.Initiative);
                if (result == 0)
                {
                    SafeGetLevel().CompareTo(otherDetails.SafeGetLevel());
                }
                return result;
            }
            return -1;
        }

        private int SafeGetLevel() { return Character?.Level ?? 0; }
    }
}

