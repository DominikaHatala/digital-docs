using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Xpdl
{
    class Reader
    {
        #region Enum

        public enum TransitionAction
        {
            None,
            SplitExclusive,
            SplitParallel,
            Join
        }

        #endregion

        #region Fields

        private PackageType m_XpdlFile;

        #endregion

        #region Constructor

        public Reader(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PackageType));

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    m_XpdlFile = (PackageType)serializer.Deserialize(reader);
                }
            }
            catch (FileNotFoundException e)
            {
                throw new ReaderException("XPDL file was not found at specified path :" + e.FileName);
            }


        }

        #endregion

        #region Public Methods

        public (TransitionAction transitionAction, List<string> nextActivities) GetActivityStatus(string activityId)
        {
            var activity = FindActivityById(activityId);

            if(activity == null)
            {
                throw new ReaderException($"Activity with specified Id : {activityId}, not found");
            }

            TransitionAction transitionAction = this.CheckActivityTransitionAction(activity);

            var nextActivities = this.GetTransitionsFromActivity(activity, transitionAction);

            return (transitionAction: transitionAction, nextActivities: nextActivities);    
        }

        #endregion

        #region Private Methods

        private Activity FindActivityById(string id)
        {
            return m_XpdlFile.WorkflowProcesses.WorkflowProcess[0].Activities.Activity.Find(activity => activity.Id.Equals(id));
        }

        private List<string> GetTransitionsFromActivity(Activity activity, TransitionAction action)
        {
            List<string> activities = new List<string>();

            switch (action)
            {
                case TransitionAction.Join:
                case TransitionAction.None:

                    var transitionId = GetSingleTransitionFromActivity(activity.Id);

                    if(!string.IsNullOrEmpty(transitionId))
                    activities.Add(GetNextActivityFromTransition(transitionId));

                    break;

                case TransitionAction.SplitParallel:
                case TransitionAction.SplitExclusive:

                    foreach (var transition in activity.TransitionRestrictions.TransitionRestriction[0].Split.TransitionRefs.TransitionRef)
                    {
                        var nextActivity = GetNextActivityFromTransition(transition.Id);
                        activities.Add(nextActivity);
                    }
                    break;
            }

            return activities;
        }

        private TransitionAction CheckActivityTransitionAction(Activity activity)
        {
            var transitionRestriction = activity.TransitionRestrictions.TransitionRestriction;

            if(transitionRestriction.Count == 0)
            {
                return TransitionAction.None;
            }

            if(transitionRestriction[0].Split.TransitionRefs.TransitionRef.Count > 0)
            {
                return transitionRestriction[0].Split.Type == SplitType.Exclusive ? TransitionAction.SplitExclusive : TransitionAction.SplitParallel;
            }
            else
            {
                return TransitionAction.Join;
            }
        }

        private string GetNextActivityFromTransition(string transitionId)
        {
            return m_XpdlFile.WorkflowProcesses.WorkflowProcess[0].Transitions.Transition.Find(transition => transition.Id.Equals(transitionId)).To;
        }

        private string GetSingleTransitionFromActivity(string activityId)
        {
            if(m_XpdlFile.WorkflowProcesses.WorkflowProcess[0].Transitions.Transition.Exists(transition => transition.From.Equals(activityId)))
            {
                return m_XpdlFile.WorkflowProcesses.WorkflowProcess[0].Transitions.Transition.Find(transition => transition.From.Equals(activityId)).Id;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

    }

    [Serializable]
    internal class ReaderException : Exception
    {
        public ReaderException()
        {
        }

        public ReaderException(string message) : base(message)
        {
        }

        public ReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
