﻿using alps.net.api.StandardPASS;
using System;

namespace alps.net.api.ALPS
{
    public interface ICommunicationRestriction : IALPSSIDComponent
    {

        void setCorrespondents(ISubject correspondentA, ISubject correspondentB, int removeCascadeDepth = 0);

        void setCorrespondentA(ISubject correspondentA, int removeCascadeDepth = 0);

        void setCorrespondentB(ISubject correspondentB, int removeCascadeDepth = 0);

        ISubject getCorrespondentA();

        ISubject getCorrespondentB();

        Tuple<ISubject, ISubject> getCorrespondents();
    }
}
