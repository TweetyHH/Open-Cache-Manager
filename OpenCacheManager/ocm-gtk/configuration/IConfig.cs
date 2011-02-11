// 
//  Copyright 2011  Kyle Campbell
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;

namespace ocmgtk
{
	public enum SolvedMode {ALL, PUZZLES, NONE};
	public interface IConfig
	{
		SolvedMode SolvedModeState {get;set;}
		double LastLat {get;set;}
		double LastLon {get;set;}
		string LastName {get;set;}
		bool UseDirectEntryMode {get;set;}
		double HomeLat {get;set;}
		double HomeLon {get;set;}
		int MapPoints {get;set;}
		string OwnerID {get;set;}
		bool ImperialUnits {get;set;}
		int WindowWidth {get;set;}
		int WindowHeight {get;set;}
		int VBarPosition {get;set;}
		int HBarPosition {get;set;}
		string MapType {get;set;}
		string DBFile {get;set;}
		string DataDirectory {get;set;}
		string ImportDirectory {get;set;}
		bool UseGPSD {get;set;}
		int GPSDPoll {get;set;}
		bool GPSDAutoMoveMap {get;set;}
		string StartupFilter {get;set;}
		bool ShowNearby {get;set;}
		bool ShowAllChildren {get;set;}
		string GPSProf {get;set;}
		bool IgnoreWaypointPrefixes {get;set;}
		bool CheckForUpdates {get;set;}
		DateTime NextUpdateCheck {get;set;}
		int UpdateInterval {get;set;}
		bool AutoCloseWindows{get;set;}
		bool AutoSelectCacheFromMap{get;set;}
		
		void CheckForDefaultGPS(GPSProfileList list, MainWindow win);
	}
}
