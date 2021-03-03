using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZGame.SkillSystem.Buffer
{
    public interface IBuffer
    {

        void OnRelease(ZGame.FSM.Entity entity);

        void OnStop();

        void Update(float dt);
    }
}