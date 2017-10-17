using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tableau.Base {

	/* A small abstraction for events involving the HoloLens gaze. */
	public class CursorEvent : UnityEvent {
		
		public Vector3 cursorPosition;

		public CursorEvent(Vector3 point) : base() {
			cursorPosition = point;
		}
	}

	public interface Gazeable {

		void OnGazeEnter(CursorEvent e);

		void OnGazeExit(CursorEvent e);

	}

	public interface Tappable {

	    void OnTapEnter(CursorEvent e);

		void OnTapExit(CursorEvent e);

	}

	public interface Draggable : Tappable {
		
		void OnDragStart(CursorEvent e);

		void OnDragEnd(CursorEvent e);

	}

