using System;
using System.Collections;
using UnityEngine;

namespace Tableau.Base {

	/* A small abstraction for events involving the HoloLens gaze. */
	public class CursorEvent : UnityEvent {
		
		public Vector3 cursorPosition;

		public CursorEvent(Vector3 point) : base() {
			cursorPosition = point;
		}
	}

	public interface Gazeable {

		public void OnGazeEnter(CursorEvent e);

		public void OnGazeExit(CursorEvent e);

	}

	public interface Tappable {

		public void OnTapEnter(CursorEvent e);

		public void OnTapExit(CursorEvent e);

	}

	public interface Draggable : Tappable {

		public void OnDragStart(CursorEvent e);

		public void OnDragEnd(CursorEvent e);

	}
}
