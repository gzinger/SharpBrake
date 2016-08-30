﻿using log4net.Core;
using log4net.Appender;
using System;
using SharpBrake.Serialization;

namespace SharpBrake {
	class Log4NetAirbrakeAppender: AppenderSkeleton {
		protected override void Append(LoggingEvent loggingEvent) {
			string sMsg = loggingEvent.RenderedMessage;
			Exception ex = loggingEvent.ExceptionObject;

			if (null == ex) {
				Exception exOut = new AirbrakeMsgException(sMsg);
				exOut.SendToAirbrake();
			} else {
				var configuration = new AirbrakeConfiguration();
				var builder = new AirbrakeNoticeBuilder(configuration);
				AirbrakeNotice notice = builder.Notice(ex);
				notice.Error.Message = sMsg + "; " + notice.Error.Message;
				var client = new AirbrakeClient();
				client.Send(notice);
			}
		}
	}
}