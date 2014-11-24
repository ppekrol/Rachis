﻿using System;
using System.IO;
using Rachis.Commands;
using Rachis.Messages;

namespace Rachis.Interfaces
{
	public interface IRaftStateMachine : IDisposable
	{
		/// <summary>
		/// This is a thread safe operation, since this is being used by both the leader's message processing thread
		/// and the leader's heartbeat thread
		/// </summary>
		long LastAppliedIndex { get; }

		void Apply(LogEntry entry, Command cmd);

		bool SupportSnapshots { get; }

		/// <summary>
		/// Create a snapshot, can be called concurrently with GetSnapshotWriter, can also be called concurrently
		/// with calls to Apply.
		/// </summary>
		void CreateSnapshot(long index, long term);

		/// <summary>
		/// Can be called concurrently with CreateSnapshot
		/// Should be cheap unless WriteSnapshot is called
		/// </summary>
		ISnapshotWriter GetSnapshotWriter();

		void ApplySnapshot(long term, long index, Stream stream);
	}
}