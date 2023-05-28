using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class HealthBarUI : HealthUIBase 
    {
        public Color32 CutOffColor { get; set; } = UnityEngine.Color.red;
        public Color32 Color { get; set; } = UnityEngine.Color.white;
        private DSpriteRendererComponent _bar;
        private DGameEntity _barEntity;

        public override DTransformComponent Transform => _barEntity.Transform;
        private const float _timeToHide = 1.5f;
        private float _hideTime = 0;

        protected override void OnAwake()
        {
            _barEntity = new DGameEntity(Name + ": HealthBar");
            _bar = _barEntity.AddComp<DSpriteRendererComponent>();

            _bar.Transform.Scale = new DVec2(1.3f, 0.07f);
            _barEntity.IsActive = false;
        }

        protected override void OnUpdate()
        {
            _bar.Color = Color;
            _bar.CutOffColor = CutOffColor;
            _bar.CutOffValue = Percentage;

            if (_hideTime > 0)
            {
                _hideTime -= DTime.DeltaTime;

                if (_hideTime <= 0)
                {
                    _barEntity.IsActive = false;
                }
            }
        }

        public void OnChancePercentage(float percentage)
        {
            _barEntity.IsActive = true;

            Percentage = percentage;
            _bar.CutOffValue = Percentage;

            _hideTime = _timeToHide;
        }

        public override void OnDestroy()
        {
            _barEntity.Destroy();
        }
    }
}