using System;
using ChatsworthLib.DataAccess;
using ChatsworthLib.Entity;
using log4net;

namespace ChatsworthLib
{
    public class MemberDirectory : IMemberDirectory
    {
        private static readonly ILog log = LogManager.GetLogger("ChatsworthLib.Logger");
        
        private ChatMemberRespository _repository;
        private ChatMemberCollection _subscribers;

        public bool AddSubscriber(string jid)
        {
            return AddSubscriber(jid, "");
        }

        public bool AddSubscriber(string jid, string alias)
        {
            if(string.IsNullOrEmpty(jid))
                throw new ArgumentNullException("jid","Calling AddSubscriber with a null or empty jid");

            var newSub = string.IsNullOrEmpty(alias) ? new ChatMember(jid) : new ChatMember(jid, alias);

            _subscribers.Add(newSub);

            AddSubscriberToDataStore(newSub);
            
            return true;
        }

        private void AddSubscriberToDataStore(ChatMember newSub)
        {
            try
            {
                if(_repository != null)
                    _repository.Save(newSub);
            }
            catch(Exception e)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat("Error saving chat member Jid:{0} Alias:{1} to repository.",newSub.Jid,newSub.Alias);
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
                if (log.IsErrorEnabled)
                    log.ErrorFormat("Error saving chat member Jid:{0} Alias:{1} to repository.", leavingSub.Jid, leavingSub.Alias);
            }
        }

        public ChatMemberCollection GetToListForSubscriber(string jid)
        {
            return _subscribers.FindAll(x => x.Jid != jid) as ChatMemberCollection;
        }

        public ChatMember LookUp(string jid)
        {
            return _subscribers.FindByJid(jid);
        }

        public ChatMemberCollection GetAllSubscribers()
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
            _subscribers = _repository.GetChatMembers();
        }
    }
}