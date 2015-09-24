using UnityEngine;
using System.Collections;

public class WallText : MonoBehaviour {
	public TextMesh Text;
	float TextAlpha = 0f;

	void Start() {
		Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, TextAlpha);
	}

	// Update is called once per frame
	void Update () {
		if(TextAlpha < 1f) {
			TextAlpha += Time.deltaTime/4f;
			if(TextAlpha > 1f)
				TextAlpha = 1f;
			Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, TextAlpha);
		}
	}
}
