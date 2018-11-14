using System;
using System.Collections.Generic;
namespace Xpdl
{
    public enum Activity
    {
        odebranie_zamowienia = 2,
        odrzucenie_zamowienia = 4,
        przygotowanie_arkusza = 5,
        uzupelnianie_zamowienia = 8,
        zatwierdzenie_oferty = 12,
        mail_klient = 13,
        wystawienie_strona = 14,
        archiwizacja = 15,
        exit = 0
    }

    class FlowManager
    {
        Reader m_Reader = new Reader("klawisze.xpdl");

        private const string ActivityNamePrefix = "klawisze_wp1_act";

        Dictionary<string, Action> m_Activities = new Dictionary<string, Action>();

        public void RegisterActivity(Activity activity, Action callback)
        {
            m_Activities.Add($"{ActivityNamePrefix}{activity.ToString()}", callback);
        }

        public void ProcessActivity(Activity startActivity, bool exclusiveStatus = true)
        {

            Activity currentActivity = startActivity;
            Action activityCallback = null;

            while(activityCallback != null)
            {

                try
                {
                    var (transitionAction, nextActivities) = m_Reader.GetActivityStatus($"{ActivityNamePrefix}{currentActivity.ToString()}");


                    switch(transitionAction)
                    {
                        case Reader.TransitionAction.SplitExclusive:
                            if(exclusiveStatus)
                            {
                                if (m_Activities.ContainsKey(nextActivities[1]))
                                {
                                    activityCallback = m_Activities[nextActivities[1]];
                                }
                                else
                                {
                                    currentActivity = nextActivities[1];
                                }
                            }
                            else
                            {
                                if (m_Activities.ContainsKey(nextActivities[0]))
                                {
                                    activityCallback = m_Activities[nextActivities[0]];
                                }
                                else
                                {
                                    currentActivity = nextActivities[0];
                                }
                            }

                            break;

                        case Reader.TransitionAction.None:
                        case Reader.TransitionAction.Join:
                        case Reader.TransitionAction.SplitParallel:
                            if (m_Activities.ContainsKey(nextActivities[0]))
                            {
                                activityCallback = m_Activities[nextActivities[0]];
                            }
                            else
                            {
                                currentActivity = nextActivities[0];
                            }

                            break;
                        default:
                            break;
                    }
                }
                catch (ReaderException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            activityCallback.Invoke();

        }


    }
}
