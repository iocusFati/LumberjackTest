using TMPro;
using UnityEngine;

namespace UI.HUDFolder
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private int _score;

        public void RaiseScore()
        {
            _score++;
            _scoreText.text = _score.ToString();
        }
    }
}