using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class MobileInput : InputService
    {
        public override Vector2 Axis =>
            SimpleInputAxis();
    }
}