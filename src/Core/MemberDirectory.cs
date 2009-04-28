using System;
using System.Collections.Generic;
using System.Linq;
using Chatsworth.Core.DataAccess;
using Chatsworth.Core.Entity;
using log4net;

namespace Chatsworth.Core
{
    public class MemberDirectory : IMemberDirectory
    {
        private ChatMemberRespository _repository;
        private List<ChatMember> _subscribers;
        private ILog _log = new NullLogger();

        public ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public bool AddSubscriber(string jid)
        {
            return AddSubscriber(jid, "");
        }

        public bool AddSubscriber(string jid, string alias)
        {
            if(string.IsNullOrEmpty(jid))
                throw new ArgumentNullException("jid", "Calling AddSubscriber with a null or empty jid");

            var newSub = string.IsNullOrEmpty(alias) ? new ChatMember(jid) : new ChatMember(jid, alias);

            _subscribers.Add(newSub);

            SaveSubscriberToDataStore(newSub);
            
            return true;
        }

        private void SaveSubscriberToDataStore(ChatMember newSub)
        {
            try
            {
                if(_repository != null)
                    _repository.Save(newSub);
            }
            catch(Exception)
            {
                if (Log.IsErrorEnabled)
                    Log.ErrorFormat("Error saving chat member Jid:{0} Alias:{1} to repository.",newSub.Jid,newSub.Alias);
            }
        }

        public bool RemoveSubscriber(string jid)
        {
            ChatMember leavingSub = _subscribers.FindByJid(jid);

            if (leavingSub != null)
            {
                RemoveSubscriberFromDataStore(leavingSub);
                _subscribers.RemoveAll(x => x.Jid == leavingSub.Jid);
            }

            return true;
        }

        private void RemoveSubscriberFromDataStore(ChatMember leavingSub)
        {
            try
            {   
                if(_repository != null)
                    _repository.Delete(leavingSub);
            }
            catch (Exception e)
            {
                if (Log.IsErrorEnabled)
                    Log.ErrorFormat("Error saving chat member Jid:{0} Alias:{1} to repository. Exception: {2}", leavingSub.Jid, e.Message);
            }
        }

        public void UpdateSubscriber(ChatMember subscriber)
        {
            if(subscriber == null)
            {
                if(Log.IsWarnEnabled)
                {
                    Log.WarnFormat("Attempted to update a null subscriber.");
                }
                return;
            }

            UpdateInMemoryCollection(subscriber);
            UpdateChatMemberInDataStore(subscriber);
        }

        void UpdateInMemoryCollection(ChatMember subscriber)
        {
            _subscribers.RemoveAll(x => x.Jid == subscriber.Jid);
            _subscribers.Add(subscriber);
        }

        private void UpdateChatMemberInDataStore(ChatMember subscriber)
        {
            try
            {
                if (_repository != null)
                    _repository.Update(subscriber);
            }
            catch (Exception)
            {
                if (Log.IsErrorEnabled)
                    Log.ErrorFormat("Error updating chat member Jid:{0} Alias:{1} to repository.", subscriber.Jid, subscriber.Alias);
            }
        }

        public List<ChatMember> GetToListForSubscriber(string jid)
        {
            return _subscribers.FindAll(x => x.Jid != jid && x.ActiveInChat);
        }

        public ChatMember LookUp(string jid)
        {
            return _subscribers.FindByJid(jid);
        }

        public List<ChatMember> GetAllSubscribers()
        {
            return _subscribers;
        }

        public ChatMember LookUpByAlias(string alias)
        {
            return _subscribers.FindByAlias(alias);
        }

        public void AttachRepository(ChatMemberRespository respository)
        {
            _repository = respository;
            _subscribers = _repository.GetChatMembers().ToList();
        }
    }
}