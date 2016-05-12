using UnityEngine;
using UnityEngine.EventSystems;

public class DotScript : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerClickHandler, IPointerDownHandler
{
	public LayerMask dragLayerMask;

	public SpriteRenderer mySpriteRenderer;
	public SpriteRenderer borderSpriteRenderer;
	public TextMesh numberText;
	public SpriteRenderer numberDotSprite;

	public Sprite[] numberSprites;

	private DotColor.ColorValue colorValue;
	private int dragLength;

	private bool dragging;

	public void OnPointerClick( PointerEventData e )
	{
		if( IsSecondaryColor( colorValue ) )
		{
			MatchMaster.Instance.CheckMatch( this );
		}
	}

	public void OnPointerDown( PointerEventData e )
	{
		if( colorValue < DotColor.ColorValue.PRIMARY_COLORS )
		{
			DragManager.Instance.StartDrag( this );
			dragging = true;
		}
	}

	public void OnDrag( PointerEventData e )
	{
		if( !dragging )
		{
			// Early return
			return;
		}
		Vector2 pos = Camera.main.ScreenToWorldPoint( e.position );
		RaycastHit2D hit = Physics2D.Raycast( pos, Vector2.zero, 0.0f, dragLayerMask );

		if( hit.collider != null )
		{
			GameObject go = hit.collider.gameObject;
			DragManager.Instance.DragOverDot( go.GetComponent<DotScript>() );
		}
	}

	public void OnPointerUp( PointerEventData e )
	{
		if( !dragging )
		{
			// Early return
			return;
		}

		DragManager.Instance.EndDrag();
	}

	public void CancelDrag()
	{
		DragManager.Instance.CancelDrag( this );
	}

	public int DragLength
	{
		get { return dragLength; }
		set { dragLength = value; }
	}

	public DotColor.ColorValue ColorValue
	{
		get { return colorValue; }
	}

	public void SetColor( DotColor.ColorValue color )
	{
		borderSpriteRenderer.enabled = false;
		colorValue = color;
		Color newColor = DotColor.GetColor( color );
		mySpriteRenderer.color = newColor;

		if( color >= DotColor.ColorValue.PRIMARY_COLORS )
		{
			SetDragLength( 0 );
		}
	}

	public void SetDragLength( int num )
	{
		dragLength = num;
		//numberText.text = (num > 0 ? "" + num : "");
		if( num > 0 )
		{
			numberDotSprite.enabled = true;
			numberDotSprite.sprite = numberSprites[ num - 1 ];
			//numberDotSprite.color = Color.white;
		}
		else
		{
			numberDotSprite.enabled = false;
		}
	}

	public void MarkToBlend( DotColor.ColorValue incomingColor )
	{
		DotColor.ColorValue blend = DotColor.GetBlend( incomingColor, colorValue );

		Color newColor = DotColor.GetColor( blend );
		mySpriteRenderer.color = DotColor.GetColor( DotColor.ColorValue.WHITE );
		borderSpriteRenderer.enabled = true;
		borderSpriteRenderer.color = newColor;

		if( IsSecondaryColor( blend ) )
		{
			//numberText.text = "";
			numberDotSprite.enabled = false;
		}
		else if( colorValue == incomingColor )
		{
			//numberText.text = "" + ( dragLength + 1 );
			numberDotSprite.sprite = numberSprites[ dragLength ];
			//numberDotSprite.color = newColor;
		}
		/*else if( colorValue == DotColor.ColorValue.WHITE )
		{
			numberText.text = "1";
		}*/
	}

	public void Unmark()
	{
		mySpriteRenderer.color = DotColor.GetColor( colorValue );
		borderSpriteRenderer.enabled = false;
		numberDotSprite.enabled = true;
		numberDotSprite.sprite = numberSprites[ dragLength - 1 ];
		//numberDotSprite.color = Color.white;
	}

	public void OnMatched()
	{
		GetComponent<Animator>().SetTrigger( "Exit" );
		GetComponent<Collider2D>().enabled = false;
	}

	public void OnExitComplete()
	{
		Destroy( gameObject );
	}

	private bool IsSecondaryColor( DotColor.ColorValue col )
	{
		return col >= DotColor.ColorValue.PRIMARY_COLORS && col < DotColor.ColorValue.SECONDARY_COLORS;
	}
}
