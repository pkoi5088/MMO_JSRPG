using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterController : CreatureController
{
	public int TemplateId { get; set; } = 1;

	protected override void Init()
	{
		base.Init();
	}

	protected override void UpdateIdle()
	{
		base.UpdateIdle();
	}

	public override void OnDamaged()
	{
		//Managers.Object.Remove(Id);
		//Managers.Resource.Destroy(gameObject);
	}

    public override void UseSkill(int skillId)
    {
		//if (skillId == 1)
		//{
		//	State = CreatureState.Skill;
		//}
    }

    protected override void UpdateAnimation()
    {
        if (_animator == null || _sprite == null)
            return;

		if (TemplateId == 1)
		{
			_animator.Play("Skull_ghost");
		}else if(TemplateId == 2)
        {
            _animator.Play("Walk");
            switch (Dir)
            {
                case MoveDir.Left:
                    _sprite.flipX = false;
                    break;
                case MoveDir.Right:
                    _sprite.flipX = true;
                    break;
            }
        }
    }
}
