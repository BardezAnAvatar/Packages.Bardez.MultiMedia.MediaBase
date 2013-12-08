using System;

namespace Bardez.Projects.Multimedia.MediaBase.Render.Audio
{
    /// <summary>This class is a unification of reverberation parameters</summary> 
    /// <remarks>The values herein will start with those defined in the I3DL2 spec</remarks>
    public class ReverbSettings
    {
        #region Helper Objects
        /// <summary>Helper class that houses default values</summary>
        public static class Defaults
        {
            /// <summary>Represents the size of the environment, in meters</summary>
            /// <remarks>
            ///     EAX 1.0 has this value specified in meters
            ///     XAudio2 has this value specified in feet
            ///     OpenAL does not have this value
            ///     Since XAudio2 has no presets, I will not try to recalculate them and just use the preset values
            /// </remarks>
            public static readonly Single EnvioronmentSize = 30.48f; //XAudio2 default is 100.0f feet = 30.48 meters

            /// <summary>Default modal density in the reverb decay, as a percent</summary>
            public static readonly Single Density = 100.0f;

            /// <summary>Constant conversion factor between feet and meters</summary>
            public static readonly Single FeetPerMeter = 3.28084f;
        }
        #endregion


        #region Fields
        /// <summary>Represents the generic size of the environment, in meters</summary>
        /// <remarks>
        ///     The rest of the code uses bels (metric) so this will be metric as well.
        ///     EAX 1.0 was in meters, and I have presets for that. XAudio2 uses feet, but has no presets.
        ///     Ergo, I will use meters and let XAudio2 convert.
        /// </remarks>
        public Single EnvironmentSize { get; set; }

        /// <summary>Represents the general attenuation of the signal within the environment</summary>
        /// <value>Clamped from -10000 to 0, in mB (hundredths of decibels)</value>
        /// <remarks>This field is named as a gain, but in reality the allowed values indicate an attenuation</remarks>
        public Int32 GeneralRoomGain { get; set; }

        /// <summary>Represents the high-frequency attenuation of the signal within the environment</summary>
        /// <value>Clamped from -10000 to 0, in mB (hundredths of decibels)</value>
        /// <remarks>This field is named as a gain, but in reality the allowed values indicate an attenuation</remarks>
        public Int32 GeneralRoomHighFrequencyGain { get; set; }

        /// <summary>Controls the rolloff of room effect intensity vs. distance, in the same way that DS3D's Rolloff Factor operates on the direct path intensity.</summary>
        /// <value>Clamped from 0.0 to 10.0</value>
        /// <remarks>
        ///     Default is 0.0, implying that the reverberation intensity does not depend on source-listener distance.
        ///     A value of 1.0 means that the reverberation decays by 6 dB per doubling of distance.
        /// </remarks>
        public Single RoomRolloffFactor { get; set; }

        /// <summary>Reverberation decay time at low frequencies</summary>
        /// <value>Clamped from 0.1 to 20.0, in seconds</value>
        public Single DecayTime { get; set; }

        /// <summary>Ratio of high-frequency decay time relative to low-frequency decay time</summary>
        /// <value>Clamped from 0.1 to 2.0</value>
        public Single DecayHighFrequencyRatio { get; set; }

        /// <summary>Adjusts intensity level of early reflections (relative to Room value)</summary>
        /// <value>Clamped from -10000 to 1000, in mB (hundredths of decibels)</value>
        public Int32 Reflections { get; set; }

        /// <summary>Delay time of the first reflection (relative to the direct path)</summary>
        /// <value>Clamped from 0.0 to 0.3, in seconds</value>
        public Single ReflectionsDelay { get; set; }

        /// <summary>Adjusts intensity of late reverberation (relative to Room value)</summary>
        /// <value>Clamped from -10000 to 2000, in mB (hundredths of decibels)</value>
        /// <remarks>
        ///     Reverb and DecayTime are independent. If the application adjusts DecayTime without changing
        ///     Reverb, the driver must keep the intensity of the late reverberation constant
        /// </remarks>
        public Int32 Reverb { get; set; }

        /// <summary>Defines the time limit between the early reflections and the late reverberation (relative to the time of the first reflection)</summary>
        /// <value>Clamped from 0.0 to 0.1, in seconds</value>
        public Single ReverbDelay { get; set; }

        /// <summary>Controls the echo density in the late reverberation decay. 0% = minimum; 100% = maximum</summary>
        /// <value>Clamped from 0 to 100, indicating a percentage</value>
        public Single Diffusion { get; set; }

        /// <summary>Controls the modal density in the late reverberation decay. 0% = minimum; 100% = maximum</summary>
        /// <value>Clamped from 0 to 100, indicating a percentage</value>
        public Single Density { get; set; }

        /// <summary>Reference high frequency</summary>
        /// <value>Clamped from 20.0 to 20000.0, in Hz</value>
        public Single HighFrequencyReference { get; set; }
        #endregion


        #region Properties
        /// <summary>Represents the generic size of the environment, in meters</summary>
        /// <remarks>
        ///     The rest of the code uses bels (metric) so this will be metric as well.
        ///     EAX 1.0 was in meters, and I have presets for that. XAudio2 uses feet, but has no presets.
        ///     Ergo, I will use meters and let XAudio2 convert.
        /// </remarks>
        public Single EnvironmentSizeFeet
        {
            get { return this.EnvironmentSize * Defaults.FeetPerMeter; }
            set { this.EnvironmentSize = value / Defaults.FeetPerMeter; }
        }
        #endregion


        #region Preset Instances Properties
        /// <summary>Collection of preset values for Reverb settings</summary>
        public static class Presets
        {
            /// <summary>Collection of I3DL2 presets</summary>
            /// <remarks>Pulled from the I3DL2 document September 20, 1999 revision 1.0a</remarks>
            public static class I3DL2
            {
                /// <summary>Default reverb settings</summary>
                public static ReverbSettings Default
                {
                    get { return new ReverbSettings(-10000, 0, 0.0f, 1.00f, 0.50f, -10000, 0.020f, -10000, 0.040f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Generic reverb settings</summary>
                public static ReverbSettings Generic
                {
                    get { return new ReverbSettings(-1000, -100, 0.0f, 1.49f, 0.83f, -2602, 0.007f, 200, 0.011f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Padded cell reverb settings</summary>
                public static ReverbSettings PaddedCell
                {
                    get { return new ReverbSettings(-1000, -6000, 0.0f, 0.17f, 0.10f, -1204, 0.001f, 207, 0.002f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Room reverb settings</summary>
                public static ReverbSettings Room
                {
                    get { return new ReverbSettings(-1000, -454, 0.0f, 0.40f, 0.83f, -1646, 0.002f, 53, 0.003f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Bathroom reverb settings</summary>
                public static ReverbSettings Bathroom
                {
                    get { return new ReverbSettings(-1000, -1200, 0.0f, 1.49f, 0.54f, -370, 0.007f, 1030, 0.011f, 100.0f, 60.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Living room reverb settings</summary>
                public static ReverbSettings LivingRoom
                {
                    get { return new ReverbSettings(-1000, -6000, 0.0f, 0.50f, 0.10f, -1376, 0.003f, -1104, 0.004f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Stone room reverb settings</summary>
                public static ReverbSettings StoneRoom
                {
                    get { return new ReverbSettings(-1000, -300, 0.0f, 2.31f, 0.64f, -711, 0.012f, 83, 0.017f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Auditorium reverb settings</summary>
                public static ReverbSettings Auditorium
                {
                    get { return new ReverbSettings(-1000, -476, 0.0f, 4.32f, 0.59f, -789, 0.020f, -289, 0.030f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Concert hall reverb settings</summary>
                public static ReverbSettings ConcertHall
                {
                    get { return new ReverbSettings(-1000, -500, 0.0f, 3.92f, 0.70f, -1230, 0.020f, -2, 0.029f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Cave reverb settings</summary>
                public static ReverbSettings Cave
                {
                    get { return new ReverbSettings(-1000, 0, 0.0f, 2.91f, 1.30f, -602, 0.015f, -302, 0.022f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Arena reverb settings</summary>
                public static ReverbSettings Arena
                {
                    get { return new ReverbSettings(-1000, -698, 0.0f, 7.24f, 0.33f, -1166, 0.020f, 16, 0.030f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Hangar reverb settings</summary>
                public static ReverbSettings Hangar
                {
                    get { return new ReverbSettings(-1000, -1000, 0.0f, 10.05f, 0.23f, -602, 0.020f, 198, 0.030f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Carpeted hallway reverb settings</summary>
                public static ReverbSettings CarpetedHallway
                {
                    get { return new ReverbSettings(-1000, -4000, 0.0f, 0.30f, 0.10f, -1831, 0.002f, -1630, 0.030f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Hallway reverb settings</summary>
                public static ReverbSettings Hallway
                {
                    get { return new ReverbSettings(-1000, -300, 0.0f, 1.49f, 0.59f, -1219, 0.007f, 441, 0.011f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Stone corridor reverb settings</summary>
                public static ReverbSettings StoneCorridor
                {
                    get { return new ReverbSettings(-1000, -237, 0.0f, 2.70f, 0.79f, -1214, 0.013f, 395, 0.020f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Alley reverb settings</summary>
                public static ReverbSettings Alley
                {
                    get { return new ReverbSettings(-1000, -270, 0.0f, 1.49f, 0.86f, -1204, 0.007f, -4, 0.011f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Forest reverb settings</summary>
                public static ReverbSettings Forest
                {
                    get { return new ReverbSettings(-1000, -3300, 0.0f, 1.49f, 0.54f, -2560, 0.162f, -613, 0.088f, 79.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>City reverb settings</summary>
                public static ReverbSettings City
                {
                    get { return new ReverbSettings(-1000, -800, 0.0f, 1.49f, 0.67f, -2273, 0.007f, -2217, 0.011f, 50.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Mountains reverb settings</summary>
                public static ReverbSettings Mountains
                {
                    get { return new ReverbSettings(-1000, -2500, 0.0f, 1.49f, 0.21f, -2780, 0.300f, -2014, 0.100f, 27.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Quarry reverb settings</summary>
                public static ReverbSettings Quarry
                {
                    get { return new ReverbSettings(-1000, -1000, 0.0f, 1.49f, 0.83f, -10000, 0.061f, 500, 0.025f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Plain reverb settings</summary>
                public static ReverbSettings Plain
                {
                    get { return new ReverbSettings(-1000, -2000, 0.0f, 1.49f, 0.50f, -2466, 0.179f, -2514, 0.100f, 21.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Parking lot reverb settings</summary>
                public static ReverbSettings ParkingLot
                {
                    get { return new ReverbSettings(-1000, 0, 0.0f, 1.65f, 1.50f, -1363, 0.008f, -1153, 0.012f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Sewer pipe reverb settings</summary>
                public static ReverbSettings SewerPipe
                {
                    get { return new ReverbSettings(-1000, -1000, 0.0f, 2.81f, 0.14f, 429, 0.014f, 648, 0.021f, 80.0f, 60.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Underwater reverb settings</summary>
                public static ReverbSettings Underwater
                {
                    get { return new ReverbSettings(-1000, -4000, 0.0f, 1.49f, 0.10f, -449, 0.007f, 1700, 0.011f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Musical small room reverb settings</summary>
                /// <remarks>A small size room with a length of 5m or so.</remarks>
                public static ReverbSettings MusicalSmallRoom
                {
                    get { return new ReverbSettings(-1000, -600, 0.0f, 1.10f, 0.83f, -400, 0.005f, 500, 0.010f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Musical medium room reverb settings</summary>
                /// <remarks>A medium size room with a length of 10m or so.</remarks>
                public static ReverbSettings MusicalMediumRoom
                {
                    get { return new ReverbSettings(-1000, -600, 0.0f, 1.30f, 0.83f, -1000, 0.010f, -200, 0.020f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Musical large room reverb settings</summary>
                /// <remarks>A large size room suitable for live performances.</remarks>
                public static ReverbSettings MusicalLargeRoom
                {
                    get { return new ReverbSettings(-1000, -600, 0.0f, 1.50f, 0.83f, -1600, 0.020f, -1000, 0.040f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Musical medium hall reverb settings</summary>
                /// <remarks>A medium size concert hall.</remarks>
                public static ReverbSettings MusicalMediumHall
                {
                    get { return new ReverbSettings(-1000, -600, 0.0f, 1.80f, 0.70f, -1300, 0.015f, -800, 0.030f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Musical large hall reverb settings</summary>
                /// <remarks>A large size concert hall suitable for a full orchestra.</remarks>
                public static ReverbSettings MusicalLargeHall
                {
                    get { return new ReverbSettings(-1000, -600, 0.0f, 1.80f, 0.70f, -2000, 0.030f, -1400, 0.060f, 100.0f, 100.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }

                /// <summary>Musical plate reverb settings</summary>
                /// <remarks>A plate reverb simulation.</remarks>
                public static ReverbSettings MusicalPlate
                {
                    get { return new ReverbSettings(-1000, -200, 0.0f, 1.30f, 0.90f, 0, 0.002f, 0, 0.010f, 100.0f, 75.0f, 5000.0f, Defaults.EnvioronmentSize); }
                }
            }

            /// <summary>Collection of EAX 1.0 presets</summary>
            /// <remarks>Pulled from OpenAL's EFX-Util.h header file</remarks>
            public static class EAX
            {
                #region Basic EAX 1.0 presets
                /// <summary>EAX 1.0 Generic preset</summary>
                public static ReverbSettings Generic
                {
                    get { return new ReverbSettings(-1000, -100, 0.00f, 1.49f, 0.83f, -2602, 0.007f, 200, 0.011f, 1.000f, Defaults.Density, 5000.0f, 7.5f); }
                }

                /// <summary>EAX 1.0 Padded Cell preset</summary>
                public static ReverbSettings PaddedCell
                {
                    get { return new ReverbSettings(-1000, -6000, 0.00f, 0.17f, 0.10f, -1204, 0.001f, 207, 0.002f, 1.000f, Defaults.Density, 5000.0f, 1.4f); }
                }

                /// <summary>EAX 1.0 Room preset</summary>
                public static ReverbSettings Room
                {
                    get { return new ReverbSettings(-1000, -454, 0.00f, 0.40f, 0.83f, -1646, 0.002f, 53, 0.003f, 1.000f, Defaults.Density, 5000.0f, 1.9f); }
                }

                /// <summary>EAX 1.0 Bathroom preset</summary>
                public static ReverbSettings Bathroom
                {
                    get { return new ReverbSettings(-1000, -1200, 0.00f, 1.49f, 0.54f, -370, 0.007f, 1030, 0.011f, 1.000f, Defaults.Density, 5000.0f, 1.4f); }
                }

                /// <summary>EAX 1.0 Living Room preset</summary>
                public static ReverbSettings LivingRoom
                {
                    get { return new ReverbSettings(-1000, -6000, 0.00f, 0.50f, 0.10f, -1376, 0.003f, -1104, 0.004f, 1.000f, Defaults.Density, 5000.0f, 2.5f); }
                }

                /// <summary>EAX 1.0 Stone Room preset</summary>
                public static ReverbSettings StoneRoom
                {
                    get { return new ReverbSettings(-1000, -300, 0.00f, 2.31f, 0.64f, -711, 0.012f, 83, 0.017f, 1.000f, Defaults.Density, 5000.0f, 11.6f); }
                }

                /// <summary>EAX 1.0 Auditorium preset</summary>
                public static ReverbSettings Auditorium
                {
                    get { return new ReverbSettings(-1000, -476, 0.00f, 4.32f, 0.59f, -789, 0.020f, -289, 0.030f, 1.000f, Defaults.Density, 5000.0f, 21.6f); }
                }

                /// <summary>EAX 1.0 Concert Hall preset</summary>
                public static ReverbSettings ConcertHall
                {
                    get { return new ReverbSettings(-1000, -500, 0.00f, 3.92f, 0.70f, -1230, 0.020f, -2, 0.029f, 1.000f, Defaults.Density, 5000.0f, 19.6f); }
                }

                /// <summary>EAX 1.0 Cave preset</summary>
                public static ReverbSettings Cave
                {
                    get { return new ReverbSettings(-1000, 0, 0.00f, 2.91f, 1.30f, -602, 0.015f, -302, 0.022f, 1.000f, Defaults.Density, 5000.0f, 14.6f); }
                }

                /// <summary>EAX 1.0 Arena preset</summary>
                public static ReverbSettings Arena
                {
                    get { return new ReverbSettings(-1000, -698, 0.00f, 7.24f, 0.33f, -1166, 0.020f, 16, 0.030f, 1.000f, Defaults.Density, 5000.0f, 36.2f); }
                }

                /// <summary>EAX 1.0 Hangar preset</summary>
                public static ReverbSettings Hangar
                {
                    get { return new ReverbSettings(-1000, -1000, 0.00f, 10.05f, 0.23f, -602, 0.020f, 198, 0.030f, 1.000f, Defaults.Density, 5000.0f, 50.3f); }
                }

                /// <summary>EAX 1.0 Carpteted Hallway preset</summary>
                public static ReverbSettings CarptetedHallway
                {
                    get { return new ReverbSettings(-1000, -4000, 0.00f, 0.30f, 0.10f, -1831, 0.002f, -1630, 0.030f, 1.000f, Defaults.Density, 5000.0f, 1.9f); }
                }

                /// <summary>EAX 1.0 Hallway preset</summary>
                public static ReverbSettings Hallway
                {
                    get { return new ReverbSettings(-1000, -300, 0.00f, 1.49f, 0.59f, -1219, 0.007f, 441, 0.011f, 1.000f, Defaults.Density, 5000.0f, 1.8f); }
                }

                /// <summary>EAX 1.0 Stone Corridor preset</summary>
                public static ReverbSettings StoneCorridor
                {
                    get { return new ReverbSettings(-1000, -237, 0.00f, 2.70f, 0.79f, -1214, 0.013f, 395, 0.020f, 1.000f, Defaults.Density, 5000.0f, 13.5f); }
                }

                /// <summary>EAX 1.0 Alley preset</summary>
                public static ReverbSettings Alley
                {
                    get { return new ReverbSettings(-1000, -270, 0.00f, 1.49f, 0.86f, -1204, 0.007f, -4, 0.011f, 0.300f, Defaults.Density, 5000.0f, 7.5f); }
                }

                /// <summary>EAX 1.0 Forest preset</summary>
                public static ReverbSettings Forest
                {
                    get { return new ReverbSettings(-1000, -3300, 0.00f, 1.49f, 0.54f, -2560, 0.162f, -229, 0.088f, 0.300f, Defaults.Density, 5000.0f, 38.0f); }
                }

                /// <summary>EAX 1.0 City preset</summary>
                public static ReverbSettings City
                {
                    get { return new ReverbSettings(-1000, -800, 0.00f, 1.49f, 0.67f, -2273, 0.007f, -1691, 0.011f, 0.500f, Defaults.Density, 5000.0f, 7.5f); }
                }

                /// <summary>EAX 1.0 Mountains preset</summary>
                public static ReverbSettings Mountains
                {
                    get { return new ReverbSettings(-1000, -2500, 0.00f, 1.49f, 0.21f, -2780, 0.300f, -1434, 0.100f, 0.270f, Defaults.Density, 5000.0f, 100.0f); }
                }

                /// <summary>EAX 1.0 Quarry preset</summary>
                public static ReverbSettings Quarry
                {
                    get { return new ReverbSettings(-1000, -1000, 0.00f, 1.49f, 0.83f, -10000, 0.061f, 500, 0.025f, 1.000f, Defaults.Density, 5000.0f, 17.5f); }
                }

                /// <summary>EAX 1.0 Plain preset</summary>
                public static ReverbSettings Plain
                {
                    get { return new ReverbSettings(-1000, -2000, 0.00f, 1.49f, 0.50f, -2466, 0.179f, -1926, 0.100f, 0.210f, Defaults.Density, 5000.0f, 42.5f); }
                }

                /// <summary>EAX 1.0 ParkingLot preset</summary>
                public static ReverbSettings ParkingLot
                {
                    get { return new ReverbSettings(-1000, 0, 0.00f, 1.65f, 1.50f, -1363, 0.008f, -1153, 0.012f, 1.000f, Defaults.Density, 5000.0f, 8.3f); }
                }

                /// <summary>EAX 1.0 Sewer Pipe preset</summary>
                public static ReverbSettings SewerPipe
                {
                    get { return new ReverbSettings(-1000, -1000, 0.00f, 2.81f, 0.14f, 429, 0.014f, 1023, 0.021f, 0.800f, Defaults.Density, 5000.0f, 1.7f); }
                }

                /// <summary>EAX 1.0 Underwater preset</summary>
                public static ReverbSettings Underwater
                {
                    get { return new ReverbSettings(-1000, -4000, 0.00f, 1.49f, 0.10f, -449, 0.007f, 1700, 0.011f, 1.000f, Defaults.Density, 5000.0f, 1.8f); }
                }

                /// <summary>EAX 1.0 Drugged preset</summary>
                public static ReverbSettings Drugged
                {
                    get { return new ReverbSettings(-1000, 0, 0.00f, 8.39f, 1.39f, -115, 0.002f, 985, 0.030f, 0.500f, Defaults.Density, 5000.0f, 1.9f); }
                }

                /// <summary>EAX 1.0 Dizzy preset</summary>
                public static ReverbSettings Dizzy
                {
                    get { return new ReverbSettings(-1000, -400, 0.00f, 17.23f, 0.56f, -1713, 0.020f, -613, 0.030f, 0.600f, Defaults.Density, 5000.0f, 1.8f); }
                }

                /// <summary>EAX 1.0 Psychotic preset</summary>
                public static ReverbSettings Psychotic
                {
                    get { return new ReverbSettings(-1000, -151, 0.00f, 7.56f, 0.91f, -626, 0.020f, 774, 0.030f, 0.500f, Defaults.Density, 5000.0f, 1.0f); }
                }
                #endregion


                #region Castle Group EAX 1.0 presets
                /// <summary>EAX 1.0 Castle Group presets</summary>
                public static class CastleGroup
                {
                    /// <summary>EAX 1.0 Castle Group Small Room preset</summary>
                    public static ReverbSettings SmallRoom
                    {
                        get { return new ReverbSettings(-1000, -800, 0.00f, 1.22f, 0.83f, -100, 0.022f, 600, 0.011f, 0.890f, Defaults.Density, 5168.6f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group Short Passage preset</summary>
                    public static ReverbSettings ShortPassage
                    {
                        get { return new ReverbSettings(-1000, -1000, 0.00f, 2.32f, 0.83f, -100, 0.007f, 200, 0.023f, 0.890f, Defaults.Density, 5168.6f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group Medium Room preset</summary>
                    public static ReverbSettings MediumRoom
                    {
                        get { return new ReverbSettings(-1000, -1100, 0.00f, 2.04f, 0.83f, -400, 0.022f, 400, 0.011f, 0.930f, Defaults.Density, 5168.6f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group LongPassage preset</summary>
                    public static ReverbSettings LongPassage
                    {
                        get { return new ReverbSettings(-1000, -800, 0.00f, 3.42f, 0.83f, -100, 0.007f, 300, 0.023f, 0.890f, Defaults.Density, 5168.6f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group Large Room preset</summary>
                    public static ReverbSettings LargeRoom
                    {
                        get { return new ReverbSettings(-1000, -1100, 0.00f, 2.53f, 0.83f, -700, 0.034f, 200, 0.016f, 0.820f, Defaults.Density, 5168.6f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group Hall preset</summary>
                    public static ReverbSettings Hall
                    {
                        get { return new ReverbSettings(-1000, -1100, 0.00f, 3.14f, 0.79f, -1500, 0.056f, 100, 0.024f, 0.810f, Defaults.Density, 5168.6f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group Cupboard preset</summary>
                    public static ReverbSettings Cupboard
                    {
                        get { return new ReverbSettings(-1000, -1100, 0.00f, 0.67f, 0.87f, 300, 0.010f, 1100, 0.007f, 0.890f, Defaults.Density, 5168.6f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group Courtyard preset</summary>
                    public static ReverbSettings Courtyard
                    {
                        get { return new ReverbSettings(-1000, -700, 0.00f, 2.13f, 0.61f, -1300, 0.160f, -300, 0.036f, 0.420f, Defaults.Density, 5000.0f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Castle Group Alcove preset</summary>
                    public static ReverbSettings Alcove
                    {
                        get { return new ReverbSettings(-1000, -600, 0.00f, 1.64f, 0.87f, 0, 0.007f, 300, 0.034f, 0.890f, Defaults.Density, 5168.6f, 8.3f); }
                    }
                }
                #endregion


                #region Factory Group EAX 1.0 presets
                /// <summary>EAX 1.0 Factory Group presets</summary>
                public static class FactoryGroup
                {
                    /// <summary>EAX 1.0 Factory Group Alcove preset</summary>
                    public static ReverbSettings Alcove
                    {
                        get { return new ReverbSettings(-1200, -200, 0.00f, 3.14f, 0.65f, 300, 0.010f, 0, 0.038f, 0.590f, Defaults.Density, 3762.6f, 1.8f); }
                    }

                    /// <summary>EAX 1.0 Factory Group Short Passage preset</summary>
                    public static ReverbSettings ShortPassage
                    {
                        get { return new ReverbSettings(-1200, -200, 0.00f, 2.53f, 0.65f, 0, 0.010f, 200, 0.038f, 0.640f, Defaults.Density, 3762.6f, 1.8f); }
                    }

                    /// <summary>EAX 1.0 Factory Group MediumRoom preset</summary>
                    public static ReverbSettings MediumRoom
                    {
                        get { return new ReverbSettings(-1200, -200, 0.00f, 2.76f, 0.65f, -1100, 0.022f, 300, 0.023f, 0.820f, Defaults.Density, 3762.6f, 1.9f); }
                    }

                    /// <summary>EAX 1.0 Factory Group Long Passage preset</summary>
                    public static ReverbSettings LongPassage
                    {
                        get { return new ReverbSettings(-1200, -200, 0.00f, 4.06f, 0.65f, 0, 0.020f, 200, 0.037f, 0.640f, Defaults.Density, 3762.6f, 1.8f); }
                    }

                    /// <summary>EAX 1.0 Factory Group LargeRoom preset</summary>
                    public static ReverbSettings LargeRoom
                    {
                        get { return new ReverbSettings(-1200, -300, 0.00f, 4.24f, 0.51f, -1500, 0.039f, 100, 0.023f, 0.750f, Defaults.Density, 3762.6f, 1.9f); }
                    }

                    /// <summary>EAX 1.0 Factory Group Hall preset</summary>
                    public static ReverbSettings Hall
                    {
                        get { return new ReverbSettings(-1000, -300, 0.00f, 7.43f, 0.51f, -2400, 0.073f, -100, 0.027f, 0.750f, Defaults.Density, 3762.6f, 1.9f); }
                    }

                    /// <summary>EAX 1.0 Factory Group Cupboard preset</summary>
                    public static ReverbSettings Cupboard
                    {
                        get { return new ReverbSettings(-1200, -200, 0.00f, 0.49f, 0.65f, 200, 0.010f, 600, 0.032f, 0.630f, Defaults.Density, 3762.6f, 1.7f); }
                    }

                    /// <summary>EAX 1.0 Factory Group Courtyard preset</summary>
                    public static ReverbSettings Courtyard
                    {
                        get { return new ReverbSettings(-1000, -1000, 0.00f, 2.32f, 0.29f, -1300, 0.140f, -800, 0.039f, 0.570f, Defaults.Density, 3762.6f, 1.7f); }
                    }

                    /// <summary>EAX 1.0 Factory Group SmallRoom preset</summary>
                    public static ReverbSettings SmallRoom
                    {
                        get { return new ReverbSettings(-1000, -200, 0.00f, 1.72f, 0.65f, -300, 0.010f, 500, 0.024f, 0.820f, Defaults.Density, 3762.6f, 1.8f); }
                    }
                }
                #endregion


                #region Ice Palace Group EAX 1.0 presets
                /// <summary>EAX 1.0 Ice Palace Group presets</summary>
                public static class IcePalaceGroup
                {
                    /// <summary>EAX 1.0 Ice Palace Group Alcove preset</summary>
                    public static ReverbSettings Alcove
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 2.76f, 1.46f, 100, 0.010f, -100, 0.030f, 0.840f, Defaults.Density, 12428.5f, 2.7f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group Short Passage preset</summary>
                    public static ReverbSettings ShortPassage
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 1.79f, 1.46f, -600, 0.010f, 100, 0.019f, 0.750f, Defaults.Density, 12428.5f, 2.7f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group Medium Room preset</summary>
                    public static ReverbSettings MediumRoom
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 2.22f, 1.53f, -800, 0.039f, 100, 0.027f, 0.870f, Defaults.Density, 12428.5f, 2.7f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group LongPassage preset</summary>
                    public static ReverbSettings LongPassage
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 3.01f, 1.46f, -200, 0.012f, 200, 0.025f, 0.770f, Defaults.Density, 12428.5f, 2.7f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group Large Room preset</summary>
                    public static ReverbSettings LargeRoom
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 3.14f, 1.53f, -1200, 0.039f, 0, 0.027f, 0.810f, Defaults.Density, 12428.5f, 2.9f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group Hall preset</summary>
                    public static ReverbSettings Hall
                    {
                        get { return new ReverbSettings(-1000, -700, 0.00f, 5.49f, 1.53f, -1900, 0.054f, -400, 0.052f, 0.760f, Defaults.Density, 12428.5f, 2.9f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group Cupboard preset</summary>
                    public static ReverbSettings Cupboard
                    {
                        get { return new ReverbSettings(-1000, -600, 0.00f, 0.76f, 1.53f, 100, 0.012f, 600, 0.016f, 0.830f, Defaults.Density, 12428.5f, 2.7f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group Courtyard preset</summary>
                    public static ReverbSettings Courtyard
                    {
                        get { return new ReverbSettings(-1000, -1100, 0.00f, 2.04f, 1.20f, -1000, 0.173f, -1000, 0.043f, 0.590f, Defaults.Density, 12428.5f, 2.9f); }
                    }

                    /// <summary>EAX 1.0 Ice Palace Group Small Room preset</summary>
                    public static ReverbSettings SmallRoom
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 1.51f, 1.53f, -100, 0.010f, 300, 0.011f, 0.840f, Defaults.Density, 12428.5f, 2.7f); }
                    }
                }
                #endregion


                #region Space Station Group EAX 1.0 presets
                /// <summary>EAX 1.0 Space Station Group presets</summary>
                public static class SpaceStationGroup
                {
                    /// <summary>EAX 1.0 Space Station Group Alcove preset</summary>
                    public static ReverbSettings Alcove
                    {
                        get { return new ReverbSettings(-1000, -300, 0.00f, 1.16f, 0.81f, 300, 0.007f, 0, 0.018f, 0.780f, Defaults.Density, 3316.1f, 1.5f); }
                    }

                    /// <summary>EAX 1.0 Space Station Group Medium Room preset</summary>
                    public static ReverbSettings MediumRoom
                    {
                        get { return new ReverbSettings(-1000, -400, 0.00f, 3.01f, 0.50f, -800, 0.034f, 100, 0.035f, 0.750f, Defaults.Density, 3316.1f, 1.5f); }
                    }

                    /// <summary>EAX 1.0 Space Station Group Short Passage preset</summary>
                    public static ReverbSettings ShortPassage
                    {
                        get { return new ReverbSettings(-1000, -400, 0.00f, 3.57f, 0.50f, 0, 0.012f, 100, 0.016f, 0.870f, Defaults.Density, 3316.1f, 1.5f); }
                    }

                    /// <summary>EAX 1.0 Space Station Group Long Passage preset</summary>
                    public static ReverbSettings LongPassage
                    {
                        get { return new ReverbSettings(-1000, -400, 0.00f, 4.62f, 0.62f, 0, 0.012f, 200, 0.031f, 0.820f, Defaults.Density, 3316.1f, 1.9f); }
                    }

                    /// <summary>EAX 1.0 Space Station Group Large Room preset</summary>
                    public static ReverbSettings LargeRoom
                    {
                        get { return new ReverbSettings(-1000, -400, 0.00f, 3.89f, 0.38f, -1000, 0.056f, -100, 0.035f, 0.810f, Defaults.Density, 3316.1f, 1.8f); }
                    }

                    /// <summary>EAX 1.0 Space Station Group Hall preset</summary>
                    public static ReverbSettings Hall
                    {
                        get { return new ReverbSettings(-1000, -400, 0.00f, 7.11f, 0.38f, -1500, 0.100f, -400, 0.047f, 0.870f, Defaults.Density, 3316.1f, 1.9f); }
                    }

                    /// <summary>EAX 1.0 Space Station Group Cupboard preset</summary>
                    public static ReverbSettings Cupboard
                    {
                        get { return new ReverbSettings(-1000, -300, 0.00f, 0.79f, 0.81f, 300, 0.007f, 500, 0.018f, 0.560f, Defaults.Density, 3316.1f, 1.4f); }
                    }

                    /// <summary>EAX 1.0 Space Station Group SmallRoom preset</summary>
                    public static ReverbSettings SmallRoom
                    {
                        get { return new ReverbSettings(-1000, -300, 0.00f, 1.72f, 0.82f, -200, 0.007f, 300, 0.013f, 0.700f, Defaults.Density, 3316.1f, 1.5f); }
                    }
                }
                #endregion


                #region Wooden Group EAX 1.0 presets
                /// <summary>EAX 1.0 Wooden Group presets</summary>
                public static class WoodenGroup
                {
                    /// <summary>EAX 1.0 Wooden Group Alcove preset</summary>
                    public static ReverbSettings Alcove
                    {
                        get { return new ReverbSettings(-1000, -1800, 0.00f, 1.22f, 0.62f, 100, 0.012f, -300, 0.024f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group Short Passage preset</summary>
                    public static ReverbSettings ShortPassage
                    {
                        get { return new ReverbSettings(-1000, -1800, 0.00f, 1.75f, 0.50f, -100, 0.012f, -400, 0.024f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group MediumRoom preset</summary>
                    public static ReverbSettings MediumRoom
                    {
                        get { return new ReverbSettings(-1000, -2000, 0.00f, 1.47f, 0.42f, -100, 0.049f, -100, 0.029f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group Long Passage preset</summary>
                    public static ReverbSettings LongPassage
                    {
                        get { return new ReverbSettings(-1000, -2000, 0.00f, 1.99f, 0.40f, 0, 0.020f, -700, 0.036f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group Large Room preset</summary>
                    public static ReverbSettings LargeRoom
                    {
                        get { return new ReverbSettings(-1000, -2100, 0.00f, 2.65f, 0.33f, -100, 0.066f, -200, 0.049f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group Hall preset</summary>
                    public static ReverbSettings Hall
                    {
                        get { return new ReverbSettings(-1000, -2200, 0.00f, 3.45f, 0.30f, -100, 0.088f, -200, 0.063f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group Cupboard preset</summary>
                    public static ReverbSettings Cupboard
                    {
                        get { return new ReverbSettings(-1000, -1700, 0.00f, 0.56f, 0.46f, 100, 0.012f, 100, 0.028f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group Small Room preset</summary>
                    public static ReverbSettings SmallRoom
                    {
                        get { return new ReverbSettings(-1000, -1900, 0.00f, 0.79f, 0.32f, 0, 0.032f, -100, 0.029f, 1.000f, Defaults.Density, 4705.0f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Wooden Group Courtyard preset</summary>
                    public static ReverbSettings Courtyard
                    {
                        get { return new ReverbSettings(-1000, -2200, 0.00f, 1.79f, 0.35f, -500, 0.123f, -2000, 0.032f, 0.650f, Defaults.Density, 4705.0f, 7.5f); }
                    }
                }
                #endregion


                #region Sport Group EAX 1.0 presets
                /// <summary>EAX 1.0 Sport Group presets</summary>
                public static class SportGroup
                {
                    /// <summary>EAX 1.0 Sport Group EmptyStadium preset</summary>
                    public static ReverbSettings EmptyStadium
                    {
                        get { return new ReverbSettings(-1000, -700, 0.00f, 6.26f, 0.51f, -2400, 0.183f, -800, 0.038f, 1.000f, Defaults.Density, 5000.0f, 7.2f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Squash Court preset</summary>
                    public static ReverbSettings SquashCourt
                    {
                        get { return new ReverbSettings(-1000, -1000, 0.00f, 2.22f, 0.91f, -700, 0.007f, -200, 0.011f, 0.750f, Defaults.Density, 7176.9f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Small Swimming Pool preset</summary>
                    public static ReverbSettings SmallSwimmingPool
                    {
                        get { return new ReverbSettings(-1000, -200, 0.00f, 2.76f, 1.25f, -400, 0.020f, -200, 0.030f, 0.700f, Defaults.Density, 5000.0f, 36.2f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Large Swimming Pool preset</summary>
                    public static ReverbSettings LargeSwimmingPool
                    {
                        get { return new ReverbSettings(-1000, -200, 0.00f, 5.49f, 1.31f, -700, 0.039f, -600, 0.049f, 0.820f, Defaults.Density, 5000.0f, 36.2f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Gymnasium preset</summary>
                    public static ReverbSettings Gymnasium
                    {
                        get { return new ReverbSettings(-1000, -700, 0.00f, 3.14f, 1.06f, -800, 0.029f, -500, 0.045f, 0.810f, Defaults.Density, 7176.9f, 7.5f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Full Stadium preset</summary>
                    public static ReverbSettings FullStadium
                    {
                        get { return new ReverbSettings(-1000, -2300, 0.00f, 5.25f, 0.17f, -2000, 0.188f, -1100, 0.038f, 1.000f, Defaults.Density, 5000.0f, 7.2f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Staduim Tannoy (loud speaker) preset</summary>
                    public static ReverbSettings StaduimTannoy
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 2.53f, 0.88f, -1100, 0.230f, -600, 0.063f, 0.780f, Defaults.Density, 5000.0f, 3.0f); }
                    }
                }
                #endregion


                #region Prefab Group EAX 1.0 presets
                /// <summary>EAX 1.0 Prefab Group presets</summary>
                public static class PrefabGroup
                {
                    /// <summary>EAX 1.0 Sport Group Workshop preset</summary>
                    public static ReverbSettings Workshop
                    {
                        get { return new ReverbSettings(-1000, -1700, 0.00f, 0.76f, 1.00f, 0, 0.012f, 100, 0.012f, 1.000f, Defaults.Density, 5000.0f, 1.9f); }
                    }

                    /// <summary>EAX 1.0 Sport Group School Room preset</summary>
                    public static ReverbSettings SchoolRoom
                    {
                        get { return new ReverbSettings(-1000, -400, 0.00f, 0.98f, 0.45f, 300, 0.017f, 300, 0.015f, 0.690f, Defaults.Density, 7176.9f, 1.86f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Practise Room preset</summary>
                    public static ReverbSettings PractiseRoom
                    {
                        get { return new ReverbSettings(-1000, -800, 0.00f, 1.12f, 0.56f, 200, 0.010f, 300, 0.011f, 0.870f, Defaults.Density, 7176.9f, 1.86f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Outhouse preset</summary>
                    public static ReverbSettings Outhouse
                    {
                        get { return new ReverbSettings(-1000, -1900, 0.00f, 1.38f, 0.38f, -100, 0.024f, -400, 0.044f, 0.820f, Defaults.Density, 2854.4f, 80.3f); }
                    }

                    /// <summary>EAX 1.0 Sport Group Caravan (trailer park trailer) preset</summary>
                    public static ReverbSettings Caravan
                    {
                        get { return new ReverbSettings(-1000, -2100, 0.00f, 0.43f, 1.50f, 0, 0.012f, 600, 0.012f, 1.000f, Defaults.Density, 5000.0f, 8.3f); }
                    }
                }
                #endregion


                #region Dome Group EAX 1.0 presets
                /// <summary>EAX 1.0 Dome Group presets</summary>
                public static class DomeGroup
                {
                    /// <summary>EAX 1.0 Dome Group Tomb preset</summary>
                    public static ReverbSettings Tomb
                    {
                        get { return new ReverbSettings(-1000, -900, 0.00f, 4.18f, 0.21f, -825, 0.030f, 450, 0.022f, 0.790f, Defaults.Density, 2854.4f, 51.8f); }
                    }

                    /// <summary>EAX 1.0 Dome Group Saint Paul's Cathedral preset</summary>
                    public static ReverbSettings SaintPaulsCathedral
                    {
                        get { return new ReverbSettings(-1000, -900, 0.00f, 10.48f, 0.19f, -1500, 0.090f, 200, 0.042f, 0.870f, Defaults.Density, 2854.4f, 50.3f); }
                    }
                }
                #endregion


                #region Pipe Group EAX 1.0 presets
                /// <summary>EAX 1.0 Pipe Group presets</summary>
                public static class PipeGroup
                {
                    /// <summary>EAX 1.0 Pipe Group Small preset</summary>
                    public static ReverbSettings Small
                    {
                        get { return new ReverbSettings(-1000, -900, 0.00f, 5.04f, 0.10f, -600, 0.032f, 800, 0.015f, 1.000f, Defaults.Density, 2854.4f, 50.3f); }
                    }

                    /// <summary>EAX 1.0 Pipe Group Long, Thin preset</summary>
                    public static ReverbSettings LongThin
                    {
                        get { return new ReverbSettings(-1000, -700, 0.00f, 9.21f, 0.18f, -300, 0.010f, -300, 0.022f, 0.910f, Defaults.Density, 2854.4f, 1.6f); }
                    }

                    /// <summary>EAX 1.0 Pipe Group Large preset</summary>
                    public static ReverbSettings Large
                    {
                        get { return new ReverbSettings(-1000, -900, 0.00f, 8.45f, 0.10f, -800, 0.046f, 400, 0.032f, 1.000f, Defaults.Density, 2854.4f, 50.3f); }
                    }

                    /// <summary>EAX 1.0 Pipe Group Resonant preset</summary>
                    public static ReverbSettings Resonant
                    {
                        get { return new ReverbSettings(-1000, -700, 0.00f, 6.81f, 0.18f, -300, 0.010f, 0, 0.022f, 0.910f, Defaults.Density, 2854.4f, 1.3f); }
                    }
                }
                #endregion


                #region Outdoors Group EAX 1.0 presets
                /// <summary>EAX 1.0 Outdoors Group presets</summary>
                public static class OutdoorsGroup
                {
                    /// <summary>EAX 1.0 Outdoors Group Backyard preset</summary>
                    public static ReverbSettings Backyard
                    {
                        get { return new ReverbSettings(-1000, -1200, 0.00f, 1.12f, 0.34f, -700, 0.069f, -300, 0.023f, 0.450f, Defaults.Density, 4399.1f, 80.3f); }
                    }

                    /// <summary>EAX 1.0 Outdoors Group Rolling Plains preset</summary>
                    public static ReverbSettings RollingPlains
                    {
                        get { return new ReverbSettings(-1000, -3900, 0.00f, 2.13f, 0.21f, -1500, 0.300f, -700, 0.019f, 0.000f, Defaults.Density, 4399.1f, 80.3f); }
                    }

                    /// <summary>EAX 1.0 Outdoors Group Deep Canyon preset</summary>
                    public static ReverbSettings DeepCanyon
                    {
                        get { return new ReverbSettings(-1000, -1500, 0.00f, 3.89f, 0.21f, -1000, 0.223f, -900, 0.019f, 0.740f, Defaults.Density, 4399.1f, 80.3f); }
                    }

                    /// <summary>EAX 1.0 Outdoors Group Creek preset</summary>
                    public static ReverbSettings Creek
                    {
                        get { return new ReverbSettings(-1000, -1500, 0.00f, 2.13f, 0.21f, -800, 0.115f, -1400, 0.031f, 0.350f, Defaults.Density, 4399.1f, 80.3f); }
                    }

                    /// <summary>EAX 1.0 Outdoors Group Valley preset</summary>
                    public static ReverbSettings Valley
                    {
                        get { return new ReverbSettings(-1000, -3100, 0.00f, 2.88f, 0.26f, -1700, 0.263f, -800, 0.100f, 0.280f, Defaults.Density, 2854.4f, 80.3f); }
                    }
                }
                #endregion


                #region Mood Group EAX 1.0 presets
                /// <summary>EAX 1.0 Mood Group presets</summary>
                public static class MoodGroup
                {
                    /// <summary>EAX 1.0 Mood Group Heaven preset</summary>
                    public static ReverbSettings Heaven
                    {
                        get { return new ReverbSettings(-1000, -200, 0.00f, 5.04f, 1.12f, -1230, 0.020f, 200, 0.029f, 0.940f, Defaults.Density, 5000.0f, 19.6f); }
                    }

                    /// <summary>EAX 1.0 Mood Group Hell preset</summary>
                    public static ReverbSettings Hell
                    {
                        get { return new ReverbSettings(-1000, -900, 0.00f, 3.57f, 0.49f, -10000, 0.020f, 300, 0.030f, 0.570f, Defaults.Density, 5000.0f, 100.0f); }
                    }

                    /// <summary>EAX 1.0 Mood Group Memory preset</summary>
                    public static ReverbSettings Memory
                    {
                        get { return new ReverbSettings(-1000, -400, 0.00f, 4.06f, 0.82f, -2800, 0.000f, 100, 0.000f, 0.850f, Defaults.Density, 5000.0f, 8.0f); }
                    }
                }
                #endregion


                #region Driving Group EAX 1.0 presets
                /// <summary>EAX 1.0 Driving Group presets</summary>
                public static class DrivingGroup
                {
                    /// <summary>EAX 1.0 Driving Group Commentator preset</summary>
                    public static ReverbSettings Commentator
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 2.42f, 0.88f, -1400, 0.093f, -1200, 0.017f, 0.000f, Defaults.Density, 5000.0f, 3.0f); }
                    }

                    /// <summary>EAX 1.0 Driving Group PitGarage preset</summary>
                    public static ReverbSettings PitGarage
                    {
                        get { return new ReverbSettings(-1000, -300, 0.00f, 1.72f, 0.93f, -500, 0.000f, 200, 0.016f, 0.590f, Defaults.Density, 5000.0f, 1.9f); }
                    }

                    /// <summary>EAX 1.0 Driving Group Full Grand Stand preset</summary>
                    public static ReverbSettings FullGrandStand
                    {
                        get { return new ReverbSettings(-1000, -1100, 0.00f, 3.01f, 1.37f, -900, 0.090f, -1500, 0.049f, 1.000f, Defaults.Density, 10420.2f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Driving Group Empty Grand Stand preset</summary>
                    public static ReverbSettings EmptyGrandStand
                    {
                        get { return new ReverbSettings(-1000, 0, 0.00f, 4.62f, 1.75f, -1363, 0.090f, -1200, 0.049f, 1.000f, Defaults.Density, 10420.2f, 8.3f); }
                    }

                    /// <summary>EAX 1.0 Driving Group Tunnel preset</summary>
                    public static ReverbSettings Tunnel
                    {
                        get { return new ReverbSettings(-1000, -800, 0.00f, 3.42f, 0.94f, -300, 0.051f, -300, 0.047f, 0.810f, Defaults.Density, 5000.0f, 3.1f); }
                    }


                    #region Driving Group EAX 1.0 In-Car presets
                    /// <summary>EAX 1.0 Driving Group In-Car presets</summary>
                    public static class InCar
                    {
                        /// <summary>EAX 1.0 Driving Group In-Car Racer preset</summary>
                        public static ReverbSettings Racer
                        {
                            get { return new ReverbSettings(-1000, 0, 0.00f, 0.17f, 2.00f, 500, 0.007f, -300, 0.015f, 0.800f, Defaults.Density, 10268.2f, 1.1f); }
                        }

                        /// <summary>EAX 1.0 Driving Group In-Car Sports preset</summary>
                        public static ReverbSettings Sports
                        {
                            get { return new ReverbSettings(-1000, -400, 0.00f, 0.17f, 0.75f, 0, 0.010f, -500, 0.000f, 0.800f, Defaults.Density, 10268.2f, 1.1f); }
                        }

                        /// <summary>EAX 1.0 Driving Group In-Car Luxury preset</summary>
                        public static ReverbSettings Luxury
                        {
                            get { return new ReverbSettings(-1000, -2000, 0.00f, 0.13f, 0.41f, -200, 0.010f, 400, 0.010f, 1.000f, Defaults.Density, 10268.2f, 1.6f); }
                        }
                    }
                    #endregion
                }
                #endregion


                #region City Group EAX 1.0 presets
                /// <summary>EAX 1.0 City Group presets</summary>
                public static class CityGroup
                {
                    /// <summary>EAX 1.0 City Group Streets preset</summary>
                    public static ReverbSettings Streets
                    {
                        get { return new ReverbSettings(-1000, -300, 0.00f, 1.79f, 1.12f, -1100, 0.046f, -1400, 0.028f, 0.780f, Defaults.Density, 5000.0f, 3.0f); }
                    }

                    /// <summary>EAX 1.0 City Group Subway preset</summary>
                    public static ReverbSettings Subway
                    {
                        get { return new ReverbSettings(-1000, -300, 0.00f, 3.01f, 1.23f, -300, 0.046f, 200, 0.028f, 0.740f, Defaults.Density, 5000.0f, 3.0f); }
                    }

                    /// <summary>EAX 1.0 City Group Museum preset</summary>
                    public static ReverbSettings Museum
                    {
                        get { return new ReverbSettings(-1000, -1500, 0.00f, 3.28f, 1.40f, -1200, 0.039f, -100, 0.034f, 0.820f, Defaults.Density, 2854.4f, 80.3f); }
                    }

                    /// <summary>EAX 1.0 City Group Library preset</summary>
                    public static ReverbSettings Library
                    {
                        get { return new ReverbSettings(-1000, -1100, 0.00f, 2.76f, 0.89f, -900, 0.029f, -100, 0.020f, 0.820f, Defaults.Density, 2854.4f, 80.3f); }
                    }

                    /// <summary>EAX 1.0 City Group Underpass preset</summary>
                    public static ReverbSettings Underpass
                    {
                        get { return new ReverbSettings(-1000, -700, 0.00f, 3.57f, 1.12f, -800, 0.059f, -100, 0.037f, 0.820f, Defaults.Density, 5000.0f, 3.0f); }
                    }

                    /// <summary>EAX 1.0 City Group Abandoned preset</summary>
                    public static ReverbSettings Abandoned
                    {
                        get { return new ReverbSettings(-1000, -200, 0.00f, 3.28f, 1.17f, -700, 0.044f, -1100, 0.024f, 0.690f, Defaults.Density, 5000.0f, 3.0f); }
                    }
                }
                #endregion


                #region Miscellaneous Group EAX 1.0 presets
                /// <summary>EAX 1.0 Miscellaneous Group presets</summary>
                public static class MiscellaneousGroup
                {
                    /// <summary>EAX 1.0 Miscellaneous Group Dusty Room preset</summary>
                    public static ReverbSettings DustyRoom
                    {
                        get { return new ReverbSettings(-1000, -200, 0.00f, 1.79f, 0.38f, -600, 0.002f, 200, 0.006f, 0.560f, Defaults.Density, 13046.0f, 1.8f); }
                    }

                    /// <summary>EAX 1.0 Miscellaneous Group Chapel preset</summary>
                    public static ReverbSettings Chapel
                    {
                        get { return new ReverbSettings(-1000, -500, 0.00f, 4.62f, 0.64f, -700, 0.032f, -200, 0.049f, 0.840f, Defaults.Density, 5000.0f, 19.6f); }
                    }

                    /// <summary>EAX 1.0 Miscellaneous Group Small Water Room preset</summary>
                    public static ReverbSettings SmallWaterRoom
                    {
                        get { return new ReverbSettings(-1000, -698, 0.00f, 1.51f, 1.25f, -100, 0.020f, 300, 0.030f, 0.700f, Defaults.Density, 5000.0f, 36.2f); }
                    }
                }
                #endregion
            }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ReverbSettings() { }

        /// <summary>Definition constructor</summary>
        /// <param name="room">General attenuation of the signal within the environment</param>
        /// <param name="roomHF">High-frequency attenuation of the signal within the environment</param>
        /// <param name="rolloffFactor">Rolloff of room effect intensity vs. distance</param>
        /// <param name="decay">Reverberation decay time at low frequencies</param>
        /// <param name="decayHFRatio">Ratio of reverberation decay time at high frequencies realtive to low frequencies</param>
        /// <param name="reflections">Adjusts intensity level of early reflections (relative to Room value)</param>
        /// <param name="reflectionsDelay">Delay time of the first reflection (relative to the direct path)</param>
        /// <param name="reverb">Adjusts intensity of late reverberation (relative to Room value)</param>
        /// <param name="reverbDelay">Defines the time limit between the early reflections and the late reverberation (relative to the time of the first reflection)</param>
        /// <param name="diffusion">Controls the echo density in the late reverberation decay</param>
        /// <param name="density">Controls the modal density in the late reverberation decay</param>
        /// <param name="hfReference">Reference high frequency</param>
        /// <param name="environmentSize">Size of the environment</param>
        public ReverbSettings(Int32 room, Int32 roomHF, Single rolloffFactor, Single decay, Single decayHFRatio, Int32 reflections, Single reflectionsDelay, Int32 reverb, Single reverbDelay, Single diffusion, Single density, Single hfReference, Single environmentSize)
        {
            this.GeneralRoomGain = room;
            this.GeneralRoomHighFrequencyGain = roomHF;
            this.RoomRolloffFactor = rolloffFactor;
            this.DecayTime = decay;
            this.DecayHighFrequencyRatio = decayHFRatio;
            this.Reflections = reflections;
            this.ReflectionsDelay = reflectionsDelay;
            this.Reverb = reverb;
            this.ReverbDelay = reverbDelay;
            this.Diffusion = diffusion;
            this.Density = density;
            this.HighFrequencyReference = hfReference;
            this.EnvironmentSize = environmentSize;
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is ReverbSettings)
                {
                    ReverbSettings compare = obj as ReverbSettings;

                    Boolean structureEquality;
                    structureEquality  = (this.EnvironmentSize == compare.EnvironmentSize);
                    structureEquality &= (this.GeneralRoomGain == compare.GeneralRoomGain);
                    structureEquality &= (this.GeneralRoomHighFrequencyGain == compare.GeneralRoomHighFrequencyGain);
                    structureEquality &= (this.RoomRolloffFactor == compare.RoomRolloffFactor);
                    structureEquality &= (this.DecayTime == compare.DecayTime);
                    structureEquality &= (this.DecayHighFrequencyRatio == compare.DecayHighFrequencyRatio);
                    structureEquality &= (this.Reflections == compare.Reflections);
                    structureEquality &= (this.ReflectionsDelay == compare.ReflectionsDelay);
                    structureEquality &= (this.Reverb == compare.Reverb);
                    structureEquality &= (this.ReverbDelay == compare.ReverbDelay);
                    structureEquality &= (this.Diffusion == compare.Diffusion);
                    structureEquality &= (this.Density == compare.Density);
                    structureEquality &= (this.HighFrequencyReference == compare.HighFrequencyReference);

                    //offsets are unimportant when it comes to data value equivalence/equality
                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = base.GetHashCode();
            hash ^= this.EnvironmentSize.GetHashCode();
            hash ^= this.GeneralRoomGain.GetHashCode();
            hash ^= this.GeneralRoomHighFrequencyGain.GetHashCode();
            hash ^= this.RoomRolloffFactor.GetHashCode();
            hash ^= this.DecayTime.GetHashCode();
            hash ^= this.DecayHighFrequencyRatio.GetHashCode();
            hash ^= this.Reflections.GetHashCode();
            hash ^= this.ReflectionsDelay.GetHashCode();
            hash ^= this.Reverb.GetHashCode();
            hash ^= this.ReverbDelay.GetHashCode();
            hash ^= this.Diffusion.GetHashCode();
            hash ^= this.Density.GetHashCode();
            hash ^= this.HighFrequencyReference.GetHashCode();

            return hash;
        }
        #endregion
    }
}