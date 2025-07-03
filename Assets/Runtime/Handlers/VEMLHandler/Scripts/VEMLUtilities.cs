// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using System;
using System.Collections.Generic;

namespace FiveSQD.WebVerse.Handlers.VEML
{
    /// <summary>
    /// Class for VEML Helper Utilities.
    /// </summary>
    public class VEMLUtilities
    {
        public static readonly string xmlHeadingTag = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        public static readonly string VEML3_0FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/3.0\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/3.0 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML2_4FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/2.4\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/2.4 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML2_3FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/2.3\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/2.3 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML2_2FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/2.2\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/2.2 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML2_1FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/2.1\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/2.1 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML2_0FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/2.0\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/2.0 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML1_3FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/1.3\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/1.3" +
            " schema.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML1_2FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/1.2\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/1.2 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML1_1FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/1.1\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/1.1 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static readonly string VEML1_0FullTag = "<veml xmlns=\"http://www.fivesqd.com/schemas/veml/1.0\"" +
            " xsi:schemaLocation=\"http://www.fivesqd.com/schemas/veml/1.0 schema.xsd\"" +
            " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";

        public static string FullyNotateVEML3_0(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML3_0FullTag);
        }

        public static string FullyNotateVEML2_4(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML2_4FullTag);
        }

        public static string FullyNotateVEML2_3(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML2_3FullTag);
        }

        public static string FullyNotateVEML2_2(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML2_2FullTag);
        }

        public static string FullyNotateVEML2_1(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML2_1FullTag);
        }

        public static string FullyNotateVEML2_0(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML2_0FullTag);
        }

        public static string FullyNotateVEML1_3(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML1_3FullTag);
        }

        public static string FullyNotateVEML1_2(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML1_2FullTag);
        }

        public static string FullyNotateVEML1_1(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML1_1FullTag);
        }

        public static string FullyNotateVEML1_0(string inputVEML)
        {
            return FullyNotateVEML(inputVEML, VEML1_0FullTag);
        }

        /// <summary>
        /// Convert the schema instance from version 3.0 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV3_0(Schema.V3_0.veml inputVEML)
        {
            // Since V3.0 is very similar to V2.4, this is mostly a direct mapping
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();

            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Direct mappings for most fields
                outputVEML.metadata.script = inputVEML.metadata.script;
                outputVEML.metadata.title = inputVEML.metadata.title;
                outputVEML.metadata.capability = inputVEML.metadata.capability;

                // Convert input events
                if (inputVEML.metadata.inputevent != null)
                {
                    List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                    foreach (Schema.V3_0.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Convert control flags - V3.0 might have new flags, map what we can
                if (inputVEML.metadata.controlflags != null)
                {
                    Schema.V2_4.controlflags outputControlFlags = new Schema.V2_4.controlflags();
                    outputControlFlags.leftvrpointer = inputVEML.metadata.controlflags.leftvrpointer;
                    outputControlFlags.rightvrpointer = inputVEML.metadata.controlflags.rightvrpointer;
                    outputControlFlags.leftvrpoker = inputVEML.metadata.controlflags.leftvrpoker;
                    outputControlFlags.rightvrpoker = inputVEML.metadata.controlflags.rightvrpoker;
                    outputControlFlags.leftvrpokerSpecified = inputVEML.metadata.controlflags.leftvrpokerSpecified;
                    outputControlFlags.rightvrpokerSpecified = inputVEML.metadata.controlflags.rightvrpokerSpecified;
                    outputControlFlags.lefthandinteraction = inputVEML.metadata.controlflags.lefthandinteraction;
                    outputControlFlags.righthandinteraction = inputVEML.metadata.controlflags.righthandinteraction;
                    outputControlFlags.lefthandinteractionSpecified = inputVEML.metadata.controlflags.lefthandinteractionSpecified;
                    outputControlFlags.righthandinteractionSpecified = inputVEML.metadata.controlflags.righthandinteractionSpecified;
                    outputControlFlags.turnlocomotion = inputVEML.metadata.controlflags.turnlocomotion;
                    outputControlFlags.joystickmotion = inputVEML.metadata.controlflags.joystickmotion;
                    outputControlFlags.joystickmotionSpecified = inputVEML.metadata.controlflags.joystickmotionSpecified;
                    outputControlFlags.leftgrabmove = inputVEML.metadata.controlflags.leftgrabmove;
                    outputControlFlags.rightgrabmove = inputVEML.metadata.controlflags.rightgrabmove;
                    outputControlFlags.leftgrabmoveSpecified = inputVEML.metadata.controlflags.leftgrabmoveSpecified;
                    outputControlFlags.rightgrabmoveSpecified = inputVEML.metadata.controlflags.rightgrabmoveSpecified;
                    outputControlFlags.twohandedgrabmove = inputVEML.metadata.controlflags.twohandedgrabmove;
                    outputControlFlags.twohandedgrabmoveSpecified = inputVEML.metadata.controlflags.twohandedgrabmoveSpecified;
                    outputVEML.metadata.controlflags = outputControlFlags;
                }

                // Convert synchronization services
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices = new List<Schema.V2_4.synchronizationservice>();
                    foreach (Schema.V3_0.synchronizationservice synchronizationService in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                        outputVEMLSynchronizationServices.Add(outputVEMLSynchronizationService);
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment - most things should map directly
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Convert background
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    
                    // Map background choice types
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V3_0.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;
                        case Schema.V3_0.ItemChoiceType.color:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                        case Schema.V3_0.ItemChoiceType.liteproceduralsky:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.liteproceduralsky;
                            // Convert lite procedural sky if needed - for now assume it's compatible
                            break;
                    }
                }

                // Convert effects
                if (inputVEML.environment.effects != null)
                {
                    outputVEML.environment.effects = new Schema.V2_4.vemlEnvironmentEffects();
                    if (inputVEML.environment.effects.litefog != null)
                    {
                        outputVEML.environment.effects.litefog = new Schema.V2_4.litefog();
                        outputVEML.environment.effects.litefog.fogenabled = inputVEML.environment.effects.litefog.fogenabled;
                        outputVEML.environment.effects.litefog.color = inputVEML.environment.effects.litefog.color;
                        outputVEML.environment.effects.litefog.density = inputVEML.environment.effects.litefog.density;
                    }
                }

                // Convert entities - this is where most of the work is since V3.0 might have new entity types
                if (inputVEML.environment.entity != null)
                {
                    List<Schema.V2_4.entity> outputEntities = new List<Schema.V2_4.entity>();
                    foreach (Schema.V3_0.entity inputEntity in inputVEML.environment.entity)
                    {
                        Schema.V2_4.entity outputEntity = ConvertV3_0EntityToV2_4(inputEntity);
                        if (outputEntity != null)
                        {
                            outputEntities.Add(outputEntity);
                        }
                    }
                    outputVEML.environment.entity = outputEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Helper method to convert V3.0 entities to V2.4 entities.
        /// </summary>
        /// <param name="inputEntity">V3.0 entity to convert.</param>
        /// <returns>V2.4 entity or null if conversion not supported.</returns>
        private static Schema.V2_4.entity ConvertV3_0EntityToV2_4(Schema.V3_0.entity inputEntity)
        {
            // Since the entity types are largely the same between V3.0 and V2.4,
            // we can do a simple type-based conversion for most cases
            
            // The conversion logic here would map specific V3.0 entity types to V2.4
            // For now, assume direct compatibility since both versions have the same entity types
            
            // This would need to be expanded based on actual V3.0 specification differences
            // For the purpose of this implementation, we'll assume entities are largely compatible
            
            return null; // TODO: Implement proper entity conversion when V3.0 spec is available
        }

        /// <summary>
        /// Convert the schema instance from version 2.3 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV2_3(Schema.V2_3.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V2_3.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign control flags.
                Schema.V2_3.controlflags outputControlFlags = new Schema.V2_3.controlflags();
                if (inputVEML.metadata.controlflags != null)
                {
                    outputControlFlags.leftvrpointer = inputVEML.metadata.controlflags.leftvrpointer.Replace("\"", "");
                    outputControlFlags.rightvrpointer = inputVEML.metadata.controlflags.rightvrpointer.Replace("\"", "");
                    outputControlFlags.leftvrpoker = inputVEML.metadata.controlflags.leftvrpoker;
                    outputControlFlags.rightvrpoker = inputVEML.metadata.controlflags.rightvrpoker;
                    outputControlFlags.leftvrpokerSpecified = inputVEML.metadata.controlflags.leftvrpokerSpecified;
                    outputControlFlags.rightvrpokerSpecified = inputVEML.metadata.controlflags.rightvrpokerSpecified;
                    outputControlFlags.lefthandinteraction = inputVEML.metadata.controlflags.lefthandinteraction;
                    outputControlFlags.righthandinteraction = inputVEML.metadata.controlflags.righthandinteraction;
                    outputControlFlags.lefthandinteractionSpecified = inputVEML.metadata.controlflags.lefthandinteractionSpecified;
                    outputControlFlags.righthandinteractionSpecified = inputVEML.metadata.controlflags.righthandinteractionSpecified;
                    outputControlFlags.turnlocomotion = inputVEML.metadata.controlflags.turnlocomotion.Replace("\"", "");
                    outputControlFlags.joystickmotion = inputVEML.metadata.controlflags.joystickmotion;
                    outputControlFlags.joystickmotionSpecified = inputVEML.metadata.controlflags.joystickmotionSpecified;
                    outputControlFlags.leftgrabmove = inputVEML.metadata.controlflags.leftgrabmove;
                    outputControlFlags.rightgrabmove = inputVEML.metadata.controlflags.rightgrabmove;
                    outputControlFlags.leftgrabmoveSpecified = inputVEML.metadata.controlflags.leftgrabmoveSpecified;
                    outputControlFlags.rightgrabmoveSpecified = inputVEML.metadata.controlflags.rightgrabmoveSpecified;
                    outputControlFlags.twohandedgrabmove = inputVEML.metadata.controlflags.twohandedgrabmove;
                    outputControlFlags.twohandedgrabmoveSpecified = inputVEML.metadata.controlflags.twohandedgrabmoveSpecified;
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V2_3.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V2_3.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V2_3.ItemChoiceType.liteproceduralsky:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.liteproceduralsky;
                            outputVEML.environment.background.Item = new Schema.V2_4.liteproceduralsky()
                            {
                                sunentitytag = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunentitytag,
                                daynightcycleenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).daynightcycleenabled,
                                groundenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).groundenabled,
                                groundcolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).groundcolor,
                                groundheight = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).groundheight,
                                groundfadeamount = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).groundfadeamount,
                                horizonskyblend = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).horizonskyblend,
                                dayhorizoncolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).dayhorizoncolor,
                                dayskycolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).dayskycolor,
                                nighthorizoncolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).nighthorizoncolor,
                                nightskycolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).nightskycolor,
                                horizonsaturationamount = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).horizonsaturationamount,
                                horizonsaturationfalloff = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).horizonsaturationfalloff,
                                sunenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunenabled,
                                sundiameter = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sundiameter,
                                sunhorizoncolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunhorizoncolor,
                                sunzenithcolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunzenithcolor,
                                sunskylightingenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunskylightingenabled,
                                skylightingfalloffamount = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).skylightingfalloffamount,
                                skylightingfalloffintensity = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).skylightingfalloffintensity,
                                sunsetintensity = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunsetintensity,
                                sunsetradialfalloff = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunsetradialfalloff,
                                sunsethorizontalfalloff = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunsethorizontalfalloff,
                                sunsetverticalfalloff = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).sunsetverticalfalloff,
                                moonenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).moonenabled,
                                moondiameter = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).moondiameter,
                                mooncolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).mooncolor,
                                moonfalloffamount = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).moonfalloffamount,
                                starsenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starsenabled,
                                starsbrightness = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starsbrightness,
                                starsdaytimebrightness = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starsdaytimebrightness,
                                starshorizonfalloff = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starshorizonfalloff,
                                starssaturation = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starssaturation,
                                proceduralstarsenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).proceduralstarsenabled,
                                proceduralstarssharpness = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).proceduralstarssharpness,
                                proceduralstarsamount = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).proceduralstarsamount,
                                starstextureenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starstextureenabled,
                                startextureuri = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).startextureuri,
                                startint = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).startint,
                                starscale = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starscale,
                                starrotationspeed = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).starrotationspeed,
                                cloudsenabled = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsenabled,
                                cloudstextureuri = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudstextureuri,
                                cloudsscalex = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsscalex,
                                cloudsscaley = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsscaley,
                                cloudsspeedx = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsspeedx,
                                cloudsspeedy = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsspeedy,
                                cloudiness = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudiness,
                                cloudsopacity = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsopacity,
                                cloudssharpness = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudssharpness,
                                cloudsshadingintensity = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsshadingintensity,
                                cloudszenithfalloff = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudszenithfalloff,
                                cloudsiterations = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsiterations,
                                cloudsgain = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsgain,
                                cloudslacunarity = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudslacunarity,
                                cloudsdaycolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsdaycolor,
                                cloudsnightcolor = ((Schema.V2_3.liteproceduralsky) inputVEML.environment.background.Item).cloudsnightcolor
                            };
                            break;
                        
                        case Schema.V2_3.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up effects.
                if (inputVEML.environment.effects != null)
                {
                    outputVEML.environment.effects = new Schema.V2_4.effectssettings();

                    if (inputVEML.environment.effects.litefog != null)
                    {
                        outputVEML.environment.effects.litefog = new Schema.V2_4.litefogsettings()
                        {
                            fogenabled = inputVEML.environment.effects.litefog.fogenabled,
                            color = inputVEML.environment.effects.litefog.color,
                            density = inputVEML.environment.effects.litefog.density
                        };
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V2_3.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V2_3.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V2_3.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V2_3.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V2_3.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV2_3(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V2_3.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V2_3.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Convert the schema instance from version 2.2 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV2_2(Schema.V2_2.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V2_2.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign control flags.
                Schema.V2_2.controlflags outputControlFlags = new Schema.V2_2.controlflags();
                if (inputVEML.metadata.controlflags != null)
                {
                    outputControlFlags.leftvrpointer = inputVEML.metadata.controlflags.leftvrpointer.Replace("\"", "");
                    outputControlFlags.rightvrpointer = inputVEML.metadata.controlflags.rightvrpointer.Replace("\"", "");
                    outputControlFlags.leftvrpoker = inputVEML.metadata.controlflags.leftvrpoker;
                    outputControlFlags.rightvrpoker = inputVEML.metadata.controlflags.rightvrpoker;
                    outputControlFlags.leftvrpokerSpecified = inputVEML.metadata.controlflags.leftvrpokerSpecified;
                    outputControlFlags.rightvrpokerSpecified = inputVEML.metadata.controlflags.rightvrpokerSpecified;
                    outputControlFlags.lefthandinteraction = inputVEML.metadata.controlflags.lefthandinteraction;
                    outputControlFlags.righthandinteraction = inputVEML.metadata.controlflags.righthandinteraction;
                    outputControlFlags.lefthandinteractionSpecified = inputVEML.metadata.controlflags.lefthandinteractionSpecified;
                    outputControlFlags.righthandinteractionSpecified = inputVEML.metadata.controlflags.righthandinteractionSpecified;
                    outputControlFlags.turnlocomotion = inputVEML.metadata.controlflags.turnlocomotion.Replace("\"", "");
                    outputControlFlags.joystickmotion = inputVEML.metadata.controlflags.joystickmotion;
                    outputControlFlags.joystickmotionSpecified = inputVEML.metadata.controlflags.joystickmotionSpecified;
                    outputControlFlags.leftgrabmove = inputVEML.metadata.controlflags.leftgrabmove;
                    outputControlFlags.rightgrabmove = inputVEML.metadata.controlflags.rightgrabmove;
                    outputControlFlags.leftgrabmoveSpecified = inputVEML.metadata.controlflags.leftgrabmoveSpecified;
                    outputControlFlags.rightgrabmoveSpecified = inputVEML.metadata.controlflags.rightgrabmoveSpecified;
                    outputControlFlags.twohandedgrabmove = inputVEML.metadata.controlflags.twohandedgrabmove;
                    outputControlFlags.twohandedgrabmoveSpecified = inputVEML.metadata.controlflags.twohandedgrabmoveSpecified;
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V2_2.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V2_2.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V2_2.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V2_2.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V2_2.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V2_2.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V2_2.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V2_2.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV2_2(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V2_2.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V2_2.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Convert the schema instance from version 2.1 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV2_1(Schema.V2_1.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V2_1.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign control flags.
                Schema.V2_1.controlflags outputControlFlags = new Schema.V2_1.controlflags();
                if (inputVEML.metadata.controlflags != null)
                {
                    outputControlFlags.leftvrpointer = inputVEML.metadata.controlflags.leftvrpointer.Replace("\"", "");
                    outputControlFlags.rightvrpointer = inputVEML.metadata.controlflags.rightvrpointer.Replace("\"", "");
                    outputControlFlags.leftvrpoker = inputVEML.metadata.controlflags.leftvrpoker;
                    outputControlFlags.rightvrpoker = inputVEML.metadata.controlflags.rightvrpoker;
                    outputControlFlags.leftvrpokerSpecified = inputVEML.metadata.controlflags.leftvrpokerSpecified;
                    outputControlFlags.rightvrpokerSpecified = inputVEML.metadata.controlflags.rightvrpokerSpecified;
                    outputControlFlags.lefthandinteraction = inputVEML.metadata.controlflags.lefthandinteraction;
                    outputControlFlags.righthandinteraction = inputVEML.metadata.controlflags.righthandinteraction;
                    outputControlFlags.lefthandinteractionSpecified = inputVEML.metadata.controlflags.lefthandinteractionSpecified;
                    outputControlFlags.righthandinteractionSpecified = inputVEML.metadata.controlflags.righthandinteractionSpecified;
                    outputControlFlags.turnlocomotion = inputVEML.metadata.controlflags.turnlocomotion.Replace("\"", "");
                    outputControlFlags.joystickmotion = inputVEML.metadata.controlflags.joystickmotion;
                    outputControlFlags.joystickmotionSpecified = inputVEML.metadata.controlflags.joystickmotionSpecified;
                    outputControlFlags.leftgrabmove = inputVEML.metadata.controlflags.leftgrabmove;
                    outputControlFlags.rightgrabmove = inputVEML.metadata.controlflags.rightgrabmove;
                    outputControlFlags.leftgrabmoveSpecified = inputVEML.metadata.controlflags.leftgrabmoveSpecified;
                    outputControlFlags.rightgrabmoveSpecified = inputVEML.metadata.controlflags.rightgrabmoveSpecified;
                    outputControlFlags.twohandedgrabmove = inputVEML.metadata.controlflags.twohandedgrabmove;
                    outputControlFlags.twohandedgrabmoveSpecified = inputVEML.metadata.controlflags.twohandedgrabmoveSpecified;
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V2_1.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V2_1.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V2_1.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V2_1.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V2_1.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V2_1.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V2_1.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V2_1.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV2_1(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V2_1.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V2_1.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Convert the schema instance from version 2.0 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV2_0(Schema.V2_0.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V2_0.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V2_0.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V2_0.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V2_0.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V2_0.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V2_0.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V2_0.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V2_0.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V2_0.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV2_0(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V2_0.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V2_0.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Convert the schema instance from version 1.3 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV1_3(Schema.V1_3.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V1_3.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V1_3.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V1_3.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V1_3.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V1_3.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V1_3.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V1_3.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V1_3.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V1_3.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV1_3(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V1_3.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V1_3.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Convert the schema instance from version 1.2 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV1_2(Schema.V1_2.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V1_2.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V1_2.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V1_2.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V1_2.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V1_2.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V1_2.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V1_2.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V1_2.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V1_2.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV1_2(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V1_2.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V1_2.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Convert the schema instance from version 1.1 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV1_1(Schema.V1_1.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V1_1.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V1_1.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V1_1.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V1_1.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V1_1.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V1_1.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V1_1.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V1_1.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V1_1.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV1_1(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V1_1.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V1_1.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Convert the schema instance from version 1.0 to the current schema (version 2.4).
        /// </summary>
        /// <param name="inputVEML">Input VEML instance.</param>
        /// <returns>Current schema version for the input VEML instance.</returns>
        public static Schema.V2_4.veml ConvertFromV1_0(Schema.V1_0.veml inputVEML)
        {
            Schema.V2_4.veml outputVEML = new Schema.V2_4.veml();
            if (inputVEML.metadata != null)
            {
                // Set up metadata.
                outputVEML.metadata = new Schema.V2_4.vemlMetadata();

                // Assign scripts.
                outputVEML.metadata.script = inputVEML.metadata.script;

                // Assign title.
                outputVEML.metadata.title = inputVEML.metadata.title;

                // Assign input events.
                List<Schema.V2_4.inputevent> outputVEMLInputEvents = new List<Schema.V2_4.inputevent>();
                if (inputVEML.metadata.inputevent != null)
                {
                    foreach (Schema.V1_0.inputevent inputEvent in inputVEML.metadata.inputevent)
                    {
                        Schema.V2_4.inputevent outputVEMLInputEvent = new Schema.V2_4.inputevent();
                        outputVEMLInputEvent.@event = inputEvent.@event;
                        outputVEMLInputEvent.input = inputEvent.input;
                        outputVEMLInputEvents.Add(outputVEMLInputEvent);
                    }
                    outputVEML.metadata.inputevent = outputVEMLInputEvents.ToArray();
                }

                // Assign synchronization services.
                List<Schema.V2_4.synchronizationservice> outputVEMLSynchronizationServices
                    = new List<Schema.V2_4.synchronizationservice>();
                if (inputVEML.metadata.synchronizationservice != null)
                {
                    foreach (Schema.V1_0.synchronizationservice synchronizationService
                        in inputVEML.metadata.synchronizationservice)
                    {
                        Schema.V2_4.synchronizationservice outputVEMLSynchronizationService
                            = new Schema.V2_4.synchronizationservice();
                        outputVEMLSynchronizationService.id = synchronizationService.id;
                        outputVEMLSynchronizationService.address = synchronizationService.address;
                        outputVEMLSynchronizationService.session = synchronizationService.session;
                        outputVEMLSynchronizationService.type = synchronizationService.type;
                    }
                    outputVEML.metadata.synchronizationservice = outputVEMLSynchronizationServices.ToArray();
                }
            }

            if (inputVEML.environment != null)
            {
                // Set up environment.
                outputVEML.environment = new Schema.V2_4.vemlEnvironment();

                // Assign background.
                if (inputVEML.environment.background != null)
                {
                    outputVEML.environment.background = new Schema.V2_4.background();
                    outputVEML.environment.background.Item = inputVEML.environment.background.Item;
                    switch (inputVEML.environment.background.ItemElementName)
                    {
                        case Schema.V1_0.ItemChoiceType.panorama:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.panorama;
                            break;

                        case Schema.V1_0.ItemChoiceType.color:
                        default:
                            outputVEML.environment.background.ItemElementName = Schema.V2_4.ItemChoiceType.color;
                            break;
                    }
                }

                // Set up entities.
                List<Schema.V2_4.entity> outputVEMLEntities = new List<Schema.V2_4.entity>();
                if (inputVEML.environment.entity != null)
                {
                    foreach (Schema.V1_0.entity e in inputVEML.environment.entity)
                    {
                        Queue<KeyValuePair<Schema.V1_0.entity, Schema.V2_4.entity>> entityQueue
                            = new Queue<KeyValuePair<Schema.V1_0.entity, Schema.V2_4.entity>>();
                        entityQueue.Enqueue(new KeyValuePair<Schema.V1_0.entity, Schema.V2_4.entity>(e, null));
                        while (entityQueue.Count > 0)
                        {
                            KeyValuePair<Schema.V1_0.entity, Schema.V2_4.entity> item = entityQueue.Dequeue();

                            Schema.V2_4.entity outputVEMLEntity = ConvertEntityFromV1_0(item.Key);

                            if (item.Key.entity1 != null)
                            {
                                foreach (Schema.V1_0.entity child in item.Key.entity1)
                                {
                                    entityQueue.Enqueue(new KeyValuePair<Schema.V1_0.entity, Schema.V2_4.entity>
                                        (child, outputVEMLEntity));
                                }
                            }

                            if (item.Value != null)
                            {
                                item.Value.entity1 = AddToV2_4EntityArray(item.Value.entity1, outputVEMLEntity);
                            }

                            if (item.Value == null)
                            {
                                outputVEMLEntities.Add(outputVEMLEntity);
                            }
                        }
                    }

                    outputVEML.environment.entity = outputVEMLEntities.ToArray();
                }
            }

            return outputVEML;
        }

        /// <summary>
        /// Parse VEML-compliant CSV formatted layer masks.
        /// </summary>
        /// <param name="csvMasks">VEML-compliant CSV formatted layer masks.</param>
        /// <returns>Terrain Entity Layer Mask Collection representation of the VEML-compliant
        /// CSV formatted layer masks.</returns>
        public static Javascript.APIs.Entity.TerrainEntityLayerMaskCollection ParseCSVLayerMasks(string csvMasks)
        {
            Javascript.APIs.Entity.TerrainEntityLayerMaskCollection telmc
                = new Javascript.APIs.Entity.TerrainEntityLayerMaskCollection();

            string[] masks = csvMasks.Split("|");
            foreach (string mask in masks)
            {
                int numCols = 0;
                string[] rows = mask.Split(";");
                int numRows = rows.Length;
                foreach (string row in rows)
                {
                    string[] cols = row.Split(",");
                    int colLength = cols.Length;
                    if (colLength > numCols)
                    {
                        numCols = colLength;
                    }
                }

                float[][] heights = new float[numRows][];
                for (int i = 0; i < numRows; i++)
                {
                    string[] cols = rows[i].Split(",");
                    heights[i] = new float[numCols];
                    for (int j = 0; j < cols.Length; j++)
                    {
                        heights[i][j] = float.Parse(cols[j]);
                    }
                }

                telmc.AddLayerMask(new Javascript.APIs.Entity.TerrainEntityLayerMask(heights));
            }

            return telmc;
        }

        /// <summary>
        /// Parse VEML-compliant CSV formatted layer masks.
        /// </summary>
        /// <param name="csvMasks">VEML-compliant CSV formatted layer masks.</param>
        /// <returns>Dictionary of indices and layer mask array representation of the VEML-compliant
        /// CSV formatted layer masks.</returns>
        public static Dictionary<int, float[,]> ParseCSVLayerMasksToInternalFormat(string csvMasks)
        {
            Dictionary<int, float[,]> outputFormat = new Dictionary<int, float[,]>();

            string[] masks = csvMasks.Split("|");
            int idx = 0;
            foreach (string mask in masks)
            {
                int numCols = 0;
                string[] rows = mask.Split(";");
                int numRows = rows.Length;
                foreach (string row in rows)
                {
                    string[] cols = row.Split(",");
                    int colLength = cols.Length;
                    if (colLength > numCols)
                    {
                        numCols = colLength;
                    }
                }

                float[,] heights = new float[numRows, numCols];
                for (int i = 0; i < numRows; i++)
                {
                    string[] cols = rows[i].Split(",");
                    for (int j = 0; j < cols.Length; j++)
                    {
                        heights[i, j] = float.Parse(cols[j]);
                    }
                }

                outputFormat.Add(idx++, heights);
            }

            return outputFormat;
        }

        /// <summary>
        /// Get a VEML-compliant CSV formatted series of layer masks.
        /// </summary>
        /// <param name="maskCollection">Terrain Entity Layer Mask Collection to format.</param>
        /// <returns>A VEML-compliant CSV formatted series of layer masks.</returns>
        public static string ToCSVLayerMasks(Javascript.APIs.Entity.TerrainEntityLayerMaskCollection maskCollection)
        {
            string outputString = "";
            foreach (Javascript.APIs.Entity.TerrainEntityLayerMask mask in maskCollection.GetLayerMasks())
            {
                int x = mask.heights.GetLength(0);
                int y = mask.heights.GetLength(1);
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (j == 0)
                        {
                            outputString = outputString + mask.heights[i, j];
                        }
                        else
                        {
                            outputString = outputString + "," + mask.heights[i, j];
                        }
                    }

                    if (i < x - 1)
                    {
                        outputString = outputString + ";";
                    }
                }

                outputString = outputString + "|";
            }

            return outputString.Substring(0, outputString.Length - 1);
        }

        /// <summary>
        /// Parse VEML-compliant CSV formatted heights.
        /// </summary>
        /// <param name="csvHeights">VEML-compliant CSV formatted heights.</param>
        /// <returns>2D float array representation of the VEML-compliant CSV formatted heights.</returns>
        public static float[,] ParseCSVHeights(string csvHeights)
        {
            int numCols = 0;
            string[] rows = csvHeights.Split(';');
            int numRows = rows.Length;
            foreach (string row in rows)
            {
                string[] cols = row.Split(",");
                int colLength = cols.Length;
                if (colLength > numCols)
                {
                    numCols = colLength;
                }
            }

            float[,] heights = new float[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                string[] cols = rows[i].Split(",");
                for (int j = 0; j < cols.Length; j++)
                {
                    heights[i, j] = float.Parse(cols[j]);
                }
            }

            return heights;
        }

        /// <summary>
        /// Parse VEML-compliant CSV formatted heights.
        /// </summary>
        /// <param name="csvHeights">VEML-compliant CSV formatted heights.</param>
        /// <returns>Array of float array representation of the VEML-compliant CSV formatted heights.</returns>
        public static float[][] ParseCSVHeightsArrayOfArray(string csvHeights)
        {
            int numCols = 0;
            string[] rows = csvHeights.Split(';');
            int numRows = rows.Length;
            foreach (string row in rows)
            {
                string[] cols = row.Split(",");
                int colLength = cols.Length;
                if (colLength > numCols)
                {
                    numCols = colLength;
                }
            }

            float[][] heights = new float[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                heights[i] = new float[numCols];
                string[] cols = rows[i].Split(",");
                for (int j = 0; j < cols.Length; j++)
                {
                    heights[i][j] = float.Parse(cols[j]);
                }
            }

            return heights;
        }

        /// <summary>
        /// Attempts to return a fully qualified URI.
        /// </summary>
        /// <param name="rawURI"></param>
        /// <param name="uriBase"></param>
        /// <returns>If URI is already fully qualified, returns
        /// raw string. Otherwise, will prepend raw string with URI base.</returns>
        public static string FullyQualifyURI(string rawURI, string uriBase)
        {
            string uriToTest = rawURI.Replace("\\", "/");
            if (uriToTest.Contains("file://") && !uriToTest.Contains("file:///"))
            {
                uriToTest = uriToTest.Replace("file://", "file:///");
            }

            if (uriToTest.Contains("file:/") && !uriToTest.Contains("file:///"))
            {
                uriToTest = uriToTest.Replace("file:/", "file:///");
            }

            if (Uri.IsWellFormedUriString(uriToTest, UriKind.Absolute))
            {
                return rawURI;
            }
            else
            {
                return uriBase + "/" + rawURI;
            }
        }

        /// <summary>
        /// Format a URI to be valid. Will replace single forward slashes in the protocol heading
        /// with double forward slashes and replace back slashes with forward slashes.
        /// </summary>
        /// <param name="unformattedURI">URI to format.</param>
        /// <returns>The formatted URI. Will replace single forward slashes in the protocol heading
        /// with double forward slashes and replace back slashes with forward slashes.</returns>
        public static string FormatURI(string unformattedURI)
        {
            string uri = unformattedURI;
            if (uri.Contains("http:/") && !uri.Contains("http://"))
            {
                uri = uri.Replace("http:/", "http://");
            }
            if (uri.Contains("http:\\") && !uri.Contains("http:\\\\"))
            {
                uri = uri.Replace("http:\\", "http://");
            }
            if (uri.Contains("https:/") && !uri.Contains("https://"))
            {
                uri = uri.Replace("https:/", "https://");
            }
            if (uri.Contains("https:\\") && !uri.Contains("https:\\\\"))
            {
                uri = uri.Replace("https:\\", "https://");
            }
            uri = uri.Replace("\\", "/");
            return uri;
        }

        private static string FullyNotateVEML(string inputVEML, string fullVEMLTag)
        {
            if (!inputVEML.StartsWith(xmlHeadingTag))
            {
                inputVEML = xmlHeadingTag + "\n" + inputVEML;
            }

            if (inputVEML.Contains("<VEML>"))
            {
                inputVEML = inputVEML.Replace("<VEML>", "<veml>").Replace("</VEML>", "</veml>");
            }

            return inputVEML.Replace("<veml>", fullVEMLTag);
        }

        /// <summary>
        /// Convert an entity from version 2.3 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV2_3(Schema.V2_3.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Arch Mesh Entity.
            if (entity is Schema.V2_3.archmeshentity)
            {
                outputEntity = new Schema.V2_4.archmeshentity();
                ((Schema.V2_4.archmeshentity) outputEntity).color
                    = ((Schema.V2_3.archmeshentity) entity).color;
            }

            // Audio Entity.
            else if (entity is Schema.V2_3.audioentity)
            {
                outputEntity = new Schema.V2_4.audioentity();
                ((Schema.V2_4.audioentity) outputEntity).audiofile
                    = ((Schema.V2_3.audioentity) entity).audiofile;
                ((Schema.V2_4.audioentity) outputEntity).autoplay
                    = ((Schema.V2_3.audioentity) entity).autoplay;
                ((Schema.V2_4.audioentity) outputEntity).loop
                    = ((Schema.V2_3.audioentity) entity).loop;
                ((Schema.V2_4.audioentity) outputEntity).priority
                    = ((Schema.V2_3.audioentity) entity).priority;
                ((Schema.V2_4.audioentity) outputEntity).volume
                    = ((Schema.V2_3.audioentity) entity).volume;
                ((Schema.V2_4.audioentity) outputEntity).pitch
                    = ((Schema.V2_3.audioentity) entity).pitch;
                ((Schema.V2_4.audioentity) outputEntity).stereopan
                    = ((Schema.V2_3.audioentity) entity).stereopan;
            }

            // Button Entity.
            else if (entity is Schema.V2_3.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V2_3.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V2_3.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Capsule Mesh Entity.
            if (entity is Schema.V2_3.capsulemeshentity)
            {
                outputEntity = new Schema.V2_4.capsulemeshentity();
                ((Schema.V2_4.capsulemeshentity) outputEntity).color
                    = ((Schema.V2_3.capsulemeshentity) entity).color;
            }

            // Character Entity.
            else if (entity is Schema.V2_3.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
                ((Schema.V2_4.characterentity) outputEntity).meshname
                    = ((Schema.V2_3.characterentity) entity).meshname;
                ((Schema.V2_4.characterentity) outputEntity).meshresource
                    = ((Schema.V2_3.characterentity) entity).meshresource;
                if (((Schema.V2_3.characterentity) entity).meshoffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.x
                        = ((Schema.V2_3.characterentity) entity).meshoffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.y
                        = ((Schema.V2_3.characterentity) entity).meshoffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.z
                        = ((Schema.V2_3.characterentity) entity).meshoffset.z;
                }
                if (((Schema.V2_3.characterentity) entity).meshrotation != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation
                        = new Schema.V2_4.rotation();
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.x
                        = ((Schema.V2_3.characterentity) entity).meshrotation.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.y
                        = ((Schema.V2_3.characterentity) entity).meshrotation.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.z
                        = ((Schema.V2_3.characterentity) entity).meshrotation.z;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.w
                        = ((Schema.V2_3.characterentity) entity).meshrotation.w;
                }
                if (((Schema.V2_3.characterentity) entity).labeloffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.x
                        = ((Schema.V2_3.characterentity) entity).labeloffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.y
                        = ((Schema.V2_3.characterentity) entity).labeloffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.z
                        = ((Schema.V2_3.characterentity) entity).labeloffset.z;
                }
            }

            // Cone Mesh Entity.
            if (entity is Schema.V2_3.conemeshentity)
            {
                outputEntity = new Schema.V2_4.conemeshentity();
                ((Schema.V2_4.conemeshentity) outputEntity).color
                    = ((Schema.V2_3.conemeshentity) entity).color;
            }

            // Container Entity.
            else if (entity is Schema.V2_3.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // Cube Mesh Entity.
            if (entity is Schema.V2_3.cubemeshentity)
            {
                outputEntity = new Schema.V2_4.cubemeshentity();
                ((Schema.V2_4.cubemeshentity) outputEntity).color
                    = ((Schema.V2_3.cubemeshentity) entity).color;
            }

            // Cylinder Mesh Entity.
            if (entity is Schema.V2_3.cylindermeshentity)
            {
                outputEntity = new Schema.V2_4.cylindermeshentity();
                ((Schema.V2_4.cylindermeshentity) outputEntity).color
                    = ((Schema.V2_3.cylindermeshentity) entity).color;
            }

            // HTML Entity.
            else if (entity is Schema.V2_3.htmlentity)
            {
                outputEntity = new Schema.V2_4.htmlentity();
                ((Schema.V2_4.htmlentity) outputEntity).onmessage
                    = ((Schema.V2_3.htmlentity) entity).onmessage;
                ((Schema.V2_4.htmlentity) outputEntity).url
                    = ((Schema.V2_3.htmlentity) entity).url;
            }

            // Image Entity.
            else if (entity is Schema.V2_3.imageentity)
            {
                outputEntity = new Schema.V2_4.imageentity();
                ((Schema.V2_4.imageentity) outputEntity).imagefile
                    = ((Schema.V2_3.imageentity) entity).imagefile;
            }

            // Input Entity.
            else if (entity is Schema.V2_3.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V2_3.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V2_3.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V2_3.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V2_3.meshentity) entity).meshname;
            }

            // Plane Mesh Entity.
            if (entity is Schema.V2_3.planemeshentity)
            {
                outputEntity = new Schema.V2_4.planemeshentity();
                ((Schema.V2_4.planemeshentity) outputEntity).color
                    = ((Schema.V2_3.planemeshentity) entity).color;
            }

            // Prism Mesh Entity.
            if (entity is Schema.V2_3.prismmeshentity)
            {
                outputEntity = new Schema.V2_4.prismmeshentity();
                ((Schema.V2_4.prismmeshentity) outputEntity).color
                    = ((Schema.V2_3.prismmeshentity) entity).color;
            }

            // Rectangular Pyramid Mesh Entity.
            if (entity is Schema.V2_3.rectangularpyramidmeshentity)
            {
                outputEntity = new Schema.V2_4.rectangularpyramidmeshentity();
                ((Schema.V2_4.rectangularpyramidmeshentity) outputEntity).color
                    = ((Schema.V2_3.rectangularpyramidmeshentity) entity).color;
            }

            // Sphere Mesh Entity.
            if (entity is Schema.V2_3.spheremeshentity)
            {
                outputEntity = new Schema.V2_4.spheremeshentity();
                ((Schema.V2_4.spheremeshentity) outputEntity).color
                    = ((Schema.V2_3.spheremeshentity) entity).color;
            }

            // Terrain Entity.
            else if (entity is Schema.V2_3.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V2_3.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V2_3.terrainentity) entity).heights;
                List<Schema.V2_4.terrainentitylayer> outputLayers
                    = new List<Schema.V2_4.terrainentitylayer>();
                if (((Schema.V2_3.terrainentity) entity).layer != null)
                {
                    foreach (Schema.V2_3.terrainentitylayer layer
                        in ((Schema.V2_3.terrainentity) entity).layer)
                    {
                        Schema.V2_4.terrainentitylayer outputLayer
                            = new Schema.V2_4.terrainentitylayer();
                        outputLayer.metallic = layer.metallic;
                        outputLayer.normaltexture = layer.normaltexture;
                        outputLayer.masktexture = layer.masktexture;
                        outputLayer.smoothness = layer.smoothness;
                        outputLayer.diffusetexture = layer.diffusetexture;
                        outputLayer.specular = layer.specular;
                        outputLayers.Add(outputLayer);
                    }
                }
                ((Schema.V2_4.terrainentity) outputEntity).layer
                    = outputLayers.ToArray();
                ((Schema.V2_4.terrainentity) outputEntity).layermasks
                    = ((Schema.V2_3.terrainentity) entity).layermasks;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V2_3.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V2_3.terrainentity) entity).width;
                ((Schema.V2_4.terrainentity) outputEntity).type
                    = ((Schema.V2_3.terrainentity) entity).type;
            }

            // Tetrahedron Mesh Entity.
            if (entity is Schema.V2_3.tetrahedronmeshentity)
            {
                outputEntity = new Schema.V2_4.tetrahedronmeshentity();
                ((Schema.V2_4.tetrahedronmeshentity) outputEntity).color
                    = ((Schema.V2_3.tetrahedronmeshentity) entity).color;
            }

            // Text Entity.
            else if (entity is Schema.V2_3.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V2_3.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V2_3.textentity) entity).fontsize;
            }

            // Torus Mesh Entity.
            if (entity is Schema.V2_3.torusmeshentity)
            {
                outputEntity = new Schema.V2_4.torusmeshentity();
                ((Schema.V2_4.torusmeshentity) outputEntity).color
                    = ((Schema.V2_3.torusmeshentity) entity).color;
            }

            // Water Blocker Entity.
            else if (entity is Schema.V2_3.waterblockerentity)
            {
                outputEntity = new Schema.V2_4.waterblockerentity();
            }

            // Water Entity.
            else if (entity is Schema.V2_3.waterentity)
            {
                outputEntity = new Schema.V2_4.waterentity();
                ((Schema.V2_4.waterentity) outputEntity).shallowcolor
                    = ((Schema.V2_3.waterentity) entity).shallowcolor;
                ((Schema.V2_4.waterentity) outputEntity).deepcolor
                    = ((Schema.V2_3.waterentity) entity).deepcolor;
                ((Schema.V2_4.waterentity) outputEntity).specularcolor
                    = ((Schema.V2_3.waterentity) entity).specularcolor;
                ((Schema.V2_4.waterentity) outputEntity).scatteringcolor
                    = ((Schema.V2_3.waterentity) entity).scatteringcolor;
                ((Schema.V2_4.waterentity) outputEntity).deepstart
                    = ((Schema.V2_3.waterentity) entity).deepstart;
                ((Schema.V2_4.waterentity) outputEntity).deepend
                    = ((Schema.V2_3.waterentity) entity).deepend;
                ((Schema.V2_4.waterentity) outputEntity).distortion
                    = ((Schema.V2_3.waterentity) entity).distortion;
                ((Schema.V2_4.waterentity) outputEntity).smoothness
                    = ((Schema.V2_3.waterentity) entity).smoothness;
                ((Schema.V2_4.waterentity) outputEntity).numwaves
                    = ((Schema.V2_3.waterentity) entity).numwaves;
                ((Schema.V2_4.waterentity) outputEntity).waveamplitude
                    = ((Schema.V2_3.waterentity) entity).waveamplitude;
                ((Schema.V2_4.waterentity) outputEntity).wavesteepness
                    = ((Schema.V2_3.waterentity) entity).wavesteepness;
                ((Schema.V2_4.waterentity) outputEntity).wavespeed
                    = ((Schema.V2_3.waterentity) entity).wavespeed;
                ((Schema.V2_4.waterentity) outputEntity).wavelength
                    = ((Schema.V2_3.waterentity) entity).wavelength;
                ((Schema.V2_4.waterentity) outputEntity).wavescale
                    = ((Schema.V2_3.waterentity) entity).wavescale;
                ((Schema.V2_4.waterentity) outputEntity).waveintensity
                    = ((Schema.V2_3.waterentity) entity).waveintensity;
            }

            // Voxel Entity.
            else if (entity is Schema.V2_3.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V2_3.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V2_3.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V2_3.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V2_3.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V2_3.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V2_3.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V2_3.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V2_3.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V2_3.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V2_3.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V2_3.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V2_3.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_3.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_3.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_3.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_3.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_3.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_3.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V2_3.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V2_3.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V2_3.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V2_3.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V2_3.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V2_3.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V2_3.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V2_3.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_3.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_3.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_3.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_3.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_3.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_3.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V2_3.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V2_3.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V2_3.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            // Assign Placement Socket.
            List<Schema.V2_4.placementsocket> outputPlacementSockets = new List<Schema.V2_4.placementsocket>();
            if (entity.placementsocket != null)
            {
                foreach (Schema.V2_3.placementsocket placementSocket in entity.placementsocket)
                {
                    Schema.V2_4.placementsocket outputPlacementSocket = new Schema.V2_4.placementsocket();
                    outputPlacementSocket.position = new Schema.V2_4.position();
                    outputPlacementSocket.position.x = placementSocket.position.x;
                    outputPlacementSocket.position.y = placementSocket.position.y;
                    outputPlacementSocket.position.z = placementSocket.position.z;
                    outputPlacementSocket.rotation = new Schema.V2_4.rotation();
                    outputPlacementSocket.rotation.x = placementSocket.rotation.x;
                    outputPlacementSocket.rotation.y = placementSocket.rotation.y;
                    outputPlacementSocket.rotation.z = placementSocket.rotation.z;
                    outputPlacementSocket.rotation.w = placementSocket.rotation.w;
                    outputPlacementSocket.connectingoffset = new Schema.V2_4.position();
                    outputPlacementSocket.connectingoffset.x = placementSocket.connectingoffset.x;
                    outputPlacementSocket.connectingoffset.y = placementSocket.connectingoffset.y;
                    outputPlacementSocket.connectingoffset.z = placementSocket.connectingoffset.z;
                    outputPlacementSockets.Add(outputPlacementSocket);
                }
            }
            outputEntity.placementsocket = outputPlacementSockets.ToArray();

            return outputEntity;
        }

        /// <summary>
        /// Convert an entity from version 2.2 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV2_2(Schema.V2_2.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Arch Mesh Entity.
            if (entity is Schema.V2_2.archmeshentity)
            {
                outputEntity = new Schema.V2_4.archmeshentity();
                ((Schema.V2_4.archmeshentity) outputEntity).color
                    = ((Schema.V2_2.archmeshentity) entity).color;
            }

            // Audio Entity.
            else if (entity is Schema.V2_2.audioentity)
            {
                outputEntity = new Schema.V2_4.audioentity();
                ((Schema.V2_4.audioentity) outputEntity).audiofile
                    = ((Schema.V2_2.audioentity) entity).audiofile;
                ((Schema.V2_4.audioentity) outputEntity).autoplay
                    = ((Schema.V2_2.audioentity) entity).autoplay;
                ((Schema.V2_4.audioentity) outputEntity).loop
                    = ((Schema.V2_2.audioentity) entity).loop;
                ((Schema.V2_4.audioentity) outputEntity).priority
                    = ((Schema.V2_2.audioentity) entity).priority;
                ((Schema.V2_4.audioentity) outputEntity).volume
                    = ((Schema.V2_2.audioentity) entity).volume;
                ((Schema.V2_4.audioentity) outputEntity).pitch
                    = ((Schema.V2_2.audioentity) entity).pitch;
                ((Schema.V2_4.audioentity) outputEntity).stereopan
                    = ((Schema.V2_2.audioentity) entity).stereopan;
            }

            // Button Entity.
            else if (entity is Schema.V2_2.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V2_2.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V2_2.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Capsule Mesh Entity.
            if (entity is Schema.V2_2.capsulemeshentity)
            {
                outputEntity = new Schema.V2_4.capsulemeshentity();
                ((Schema.V2_4.capsulemeshentity) outputEntity).color
                    = ((Schema.V2_2.capsulemeshentity) entity).color;
            }

            // Character Entity.
            else if (entity is Schema.V2_2.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
                ((Schema.V2_4.characterentity) outputEntity).meshname
                    = ((Schema.V2_2.characterentity) entity).meshname;
                ((Schema.V2_4.characterentity) outputEntity).meshresource
                    = ((Schema.V2_2.characterentity) entity).meshresource;
                if (((Schema.V2_2.characterentity) entity).meshoffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.x
                        = ((Schema.V2_2.characterentity) entity).meshoffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.y
                        = ((Schema.V2_2.characterentity) entity).meshoffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.z
                        = ((Schema.V2_2.characterentity) entity).meshoffset.z;
                }
                if (((Schema.V2_2.characterentity) entity).meshrotation != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation
                        = new Schema.V2_4.rotation();
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.x
                        = ((Schema.V2_2.characterentity) entity).meshrotation.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.y
                        = ((Schema.V2_2.characterentity) entity).meshrotation.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.z
                        = ((Schema.V2_2.characterentity) entity).meshrotation.z;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.w
                        = ((Schema.V2_2.characterentity) entity).meshrotation.w;
                }
                if (((Schema.V2_2.characterentity) entity).labeloffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.x
                        = ((Schema.V2_2.characterentity) entity).labeloffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.y
                        = ((Schema.V2_2.characterentity) entity).labeloffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.z
                        = ((Schema.V2_2.characterentity) entity).labeloffset.z;
                }
            }

            // Cone Mesh Entity.
            if (entity is Schema.V2_2.conemeshentity)
            {
                outputEntity = new Schema.V2_4.conemeshentity();
                ((Schema.V2_4.conemeshentity) outputEntity).color
                    = ((Schema.V2_2.conemeshentity) entity).color;
            }

            // Container Entity.
            else if (entity is Schema.V2_2.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // Cube Mesh Entity.
            if (entity is Schema.V2_2.cubemeshentity)
            {
                outputEntity = new Schema.V2_4.cubemeshentity();
                ((Schema.V2_4.cubemeshentity) outputEntity).color
                    = ((Schema.V2_2.cubemeshentity) entity).color;
            }

            // Cylinder Mesh Entity.
            if (entity is Schema.V2_2.cylindermeshentity)
            {
                outputEntity = new Schema.V2_4.cylindermeshentity();
                ((Schema.V2_4.cylindermeshentity) outputEntity).color
                    = ((Schema.V2_2.cylindermeshentity) entity).color;
            }

            // HTML Entity.
            else if (entity is Schema.V2_2.htmlentity)
            {
                outputEntity = new Schema.V2_4.htmlentity();
                ((Schema.V2_4.htmlentity) outputEntity).onmessage
                    = ((Schema.V2_2.htmlentity) entity).onmessage;
                ((Schema.V2_4.htmlentity) outputEntity).url
                    = ((Schema.V2_2.htmlentity) entity).url;
            }

            // Image Entity.
            else if (entity is Schema.V2_2.imageentity)
            {
                outputEntity = new Schema.V2_4.imageentity();
                ((Schema.V2_4.imageentity) outputEntity).imagefile
                    = ((Schema.V2_2.imageentity) entity).imagefile;
            }

            // Input Entity.
            else if (entity is Schema.V2_2.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V2_2.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V2_2.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V2_2.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V2_2.meshentity) entity).meshname;
            }

            // Plane Mesh Entity.
            if (entity is Schema.V2_2.planemeshentity)
            {
                outputEntity = new Schema.V2_4.planemeshentity();
                ((Schema.V2_4.planemeshentity) outputEntity).color
                    = ((Schema.V2_2.planemeshentity) entity).color;
            }

            // Prism Mesh Entity.
            if (entity is Schema.V2_2.prismmeshentity)
            {
                outputEntity = new Schema.V2_4.prismmeshentity();
                ((Schema.V2_4.prismmeshentity) outputEntity).color
                    = ((Schema.V2_2.prismmeshentity) entity).color;
            }

            // Rectangular Pyramid Mesh Entity.
            if (entity is Schema.V2_2.rectangularpyramidmeshentity)
            {
                outputEntity = new Schema.V2_4.rectangularpyramidmeshentity();
                ((Schema.V2_4.rectangularpyramidmeshentity) outputEntity).color
                    = ((Schema.V2_2.rectangularpyramidmeshentity) entity).color;
            }

            // Sphere Mesh Entity.
            if (entity is Schema.V2_2.spheremeshentity)
            {
                outputEntity = new Schema.V2_4.spheremeshentity();
                ((Schema.V2_4.spheremeshentity) outputEntity).color
                    = ((Schema.V2_2.spheremeshentity) entity).color;
            }

            // Terrain Entity.
            else if (entity is Schema.V2_2.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V2_2.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V2_2.terrainentity) entity).heights;
                List<Schema.V2_4.terrainentitylayer> outputLayers
                    = new List<Schema.V2_4.terrainentitylayer>();
                if (((Schema.V2_2.terrainentity) entity).layer != null)
                {
                    foreach (Schema.V2_2.terrainentitylayer layer
                        in ((Schema.V2_2.terrainentity) entity).layer)
                    {
                        Schema.V2_4.terrainentitylayer outputLayer
                            = new Schema.V2_4.terrainentitylayer();
                        outputLayer.metallic = layer.metallic;
                        outputLayer.normaltexture = layer.normaltexture;
                        outputLayer.masktexture = layer.masktexture;
                        outputLayer.smoothness = layer.smoothness;
                        outputLayer.diffusetexture = layer.diffusetexture;
                        outputLayer.specular = layer.specular;
                        outputLayers.Add(outputLayer);
                    }
                }
                ((Schema.V2_4.terrainentity) outputEntity).layer
                    = outputLayers.ToArray();
                ((Schema.V2_4.terrainentity) outputEntity).layermasks
                    = ((Schema.V2_2.terrainentity) entity).layermasks;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V2_2.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V2_2.terrainentity) entity).width;
                ((Schema.V2_4.terrainentity) outputEntity).type
                    = ((Schema.V2_2.terrainentity) entity).type;
            }

            // Tetrahedron Mesh Entity.
            if (entity is Schema.V2_2.tetrahedronmeshentity)
            {
                outputEntity = new Schema.V2_4.tetrahedronmeshentity();
                ((Schema.V2_4.tetrahedronmeshentity) outputEntity).color
                    = ((Schema.V2_2.tetrahedronmeshentity) entity).color;
            }

            // Text Entity.
            else if (entity is Schema.V2_2.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V2_2.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V2_2.textentity) entity).fontsize;
            }

            // Torus Mesh Entity.
            if (entity is Schema.V2_2.torusmeshentity)
            {
                outputEntity = new Schema.V2_4.torusmeshentity();
                ((Schema.V2_4.torusmeshentity) outputEntity).color
                    = ((Schema.V2_2.torusmeshentity) entity).color;
            }

            // Water Blocker Entity.
            else if (entity is Schema.V2_2.waterblockerentity)
            {
                outputEntity = new Schema.V2_4.waterblockerentity();
            }

            // Water Entity.
            else if (entity is Schema.V2_2.waterentity)
            {
                outputEntity = new Schema.V2_4.waterentity();
                ((Schema.V2_4.waterentity) outputEntity).shallowcolor
                    = ((Schema.V2_2.waterentity) entity).shallowcolor;
                ((Schema.V2_4.waterentity) outputEntity).deepcolor
                    = ((Schema.V2_2.waterentity) entity).deepcolor;
                ((Schema.V2_4.waterentity) outputEntity).specularcolor
                    = ((Schema.V2_2.waterentity) entity).specularcolor;
                ((Schema.V2_4.waterentity) outputEntity).scatteringcolor
                    = ((Schema.V2_2.waterentity) entity).scatteringcolor;
                ((Schema.V2_4.waterentity) outputEntity).deepstart
                    = ((Schema.V2_2.waterentity) entity).deepstart;
                ((Schema.V2_4.waterentity) outputEntity).deepend
                    = ((Schema.V2_2.waterentity) entity).deepend;
                ((Schema.V2_4.waterentity) outputEntity).distortion
                    = ((Schema.V2_2.waterentity) entity).distortion;
                ((Schema.V2_4.waterentity) outputEntity).smoothness
                    = ((Schema.V2_2.waterentity) entity).smoothness;
                ((Schema.V2_4.waterentity) outputEntity).numwaves
                    = ((Schema.V2_2.waterentity) entity).numwaves;
                ((Schema.V2_4.waterentity) outputEntity).waveamplitude
                    = ((Schema.V2_2.waterentity) entity).waveamplitude;
                ((Schema.V2_4.waterentity) outputEntity).wavesteepness
                    = ((Schema.V2_2.waterentity) entity).wavesteepness;
                ((Schema.V2_4.waterentity) outputEntity).wavespeed
                    = ((Schema.V2_2.waterentity) entity).wavespeed;
                ((Schema.V2_4.waterentity) outputEntity).wavelength
                    = ((Schema.V2_2.waterentity) entity).wavelength;
                ((Schema.V2_4.waterentity) outputEntity).wavescale
                    = ((Schema.V2_2.waterentity) entity).wavescale;
                ((Schema.V2_4.waterentity) outputEntity).waveintensity
                    = ((Schema.V2_2.waterentity) entity).waveintensity;
            }

            // Voxel Entity.
            else if (entity is Schema.V2_2.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V2_2.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V2_2.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V2_2.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V2_2.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V2_2.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V2_2.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V2_2.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V2_2.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V2_2.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V2_2.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V2_2.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V2_2.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_2.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_2.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_2.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_2.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_2.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_2.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V2_2.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V2_2.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V2_2.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V2_2.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V2_2.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V2_2.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V2_2.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V2_2.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_2.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_2.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_2.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_2.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_2.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_2.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V2_2.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V2_2.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V2_2.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            // Assign Placement Socket.
            List<Schema.V2_4.placementsocket> outputPlacementSockets = new List<Schema.V2_4.placementsocket>();
            if (entity.placementsocket != null)
            {
                foreach (Schema.V2_2.placementsocket placementSocket in entity.placementsocket)
                {
                    Schema.V2_4.placementsocket outputPlacementSocket = new Schema.V2_4.placementsocket();
                    outputPlacementSocket.position = new Schema.V2_4.position();
                    outputPlacementSocket.position.x = placementSocket.position.x;
                    outputPlacementSocket.position.y = placementSocket.position.y;
                    outputPlacementSocket.position.z = placementSocket.position.z;
                    outputPlacementSocket.rotation = new Schema.V2_4.rotation();
                    outputPlacementSocket.rotation.x = placementSocket.rotation.x;
                    outputPlacementSocket.rotation.y = placementSocket.rotation.y;
                    outputPlacementSocket.rotation.z = placementSocket.rotation.z;
                    outputPlacementSocket.rotation.w = placementSocket.rotation.w;
                    outputPlacementSocket.connectingoffset = new Schema.V2_4.position();
                    outputPlacementSocket.connectingoffset.x = placementSocket.connectingoffset.x;
                    outputPlacementSocket.connectingoffset.y = placementSocket.connectingoffset.y;
                    outputPlacementSocket.connectingoffset.z = placementSocket.connectingoffset.z;
                    outputPlacementSockets.Add(outputPlacementSocket);
                }
            }
            outputEntity.placementsocket = outputPlacementSockets.ToArray();

            return outputEntity;
        }

        /// <summary>
        /// Convert an entity from version 2.1 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV2_1(Schema.V2_1.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Arch Mesh Entity.
            if (entity is Schema.V2_1.archmeshentity)
            {
                outputEntity = new Schema.V2_4.archmeshentity();
                ((Schema.V2_4.archmeshentity) outputEntity).color
                    = ((Schema.V2_1.archmeshentity) entity).color;
            }

            // Audio Entity.
            else if (entity is Schema.V2_1.audioentity)
            {
                outputEntity = new Schema.V2_4.audioentity();
                ((Schema.V2_4.audioentity) outputEntity).audiofile
                    = ((Schema.V2_1.audioentity) entity).audiofile;
                ((Schema.V2_4.audioentity) outputEntity).autoplay
                    = ((Schema.V2_1.audioentity) entity).autoplay;
                ((Schema.V2_4.audioentity) outputEntity).loop
                    = ((Schema.V2_1.audioentity) entity).loop;
                ((Schema.V2_4.audioentity) outputEntity).priority
                    = ((Schema.V2_1.audioentity) entity).priority;
                ((Schema.V2_4.audioentity) outputEntity).volume
                    = ((Schema.V2_1.audioentity) entity).volume;
                ((Schema.V2_4.audioentity) outputEntity).pitch
                    = ((Schema.V2_1.audioentity) entity).pitch;
                ((Schema.V2_4.audioentity) outputEntity).stereopan
                    = ((Schema.V2_1.audioentity) entity).stereopan;
            }

            // Button Entity.
            else if (entity is Schema.V2_1.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V2_1.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V2_1.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Capsule Mesh Entity.
            if (entity is Schema.V2_1.capsulemeshentity)
            {
                outputEntity = new Schema.V2_4.capsulemeshentity();
                ((Schema.V2_4.capsulemeshentity) outputEntity).color
                    = ((Schema.V2_1.capsulemeshentity) entity).color;
            }

            // Character Entity.
            else if (entity is Schema.V2_1.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
                ((Schema.V2_4.characterentity) outputEntity).meshname
                    = ((Schema.V2_1.characterentity) entity).meshname;
                ((Schema.V2_4.characterentity) outputEntity).meshresource
                    = ((Schema.V2_1.characterentity) entity).meshresource;
                if (((Schema.V2_1.characterentity) entity).meshoffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.x
                        = ((Schema.V2_1.characterentity) entity).meshoffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.y
                        = ((Schema.V2_1.characterentity) entity).meshoffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.z
                        = ((Schema.V2_1.characterentity) entity).meshoffset.z;
                }
                if (((Schema.V2_1.characterentity) entity).meshrotation != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation
                        = new Schema.V2_4.rotation();
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.x
                        = ((Schema.V2_1.characterentity) entity).meshrotation.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.y
                        = ((Schema.V2_1.characterentity) entity).meshrotation.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.z
                        = ((Schema.V2_1.characterentity) entity).meshrotation.z;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.w
                        = ((Schema.V2_1.characterentity) entity).meshrotation.w;
                }
                if (((Schema.V2_1.characterentity) entity).labeloffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.x
                        = ((Schema.V2_1.characterentity) entity).labeloffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.y
                        = ((Schema.V2_1.characterentity) entity).labeloffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.z
                        = ((Schema.V2_1.characterentity) entity).labeloffset.z;
                }
            }

            // Cone Mesh Entity.
            if (entity is Schema.V2_1.conemeshentity)
            {
                outputEntity = new Schema.V2_4.conemeshentity();
                ((Schema.V2_4.conemeshentity) outputEntity).color
                    = ((Schema.V2_1.conemeshentity) entity).color;
            }

            // Container Entity.
            else if (entity is Schema.V2_1.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // Cube Mesh Entity.
            if (entity is Schema.V2_1.cubemeshentity)
            {
                outputEntity = new Schema.V2_4.cubemeshentity();
                ((Schema.V2_4.cubemeshentity) outputEntity).color
                    = ((Schema.V2_1.cubemeshentity) entity).color;
            }

            // Cylinder Mesh Entity.
            if (entity is Schema.V2_1.cylindermeshentity)
            {
                outputEntity = new Schema.V2_4.cylindermeshentity();
                ((Schema.V2_4.cylindermeshentity) outputEntity).color
                    = ((Schema.V2_1.cylindermeshentity) entity).color;
            }

            // HTML Entity.
            else if (entity is Schema.V2_1.htmlentity)
            {
                outputEntity = new Schema.V2_4.htmlentity();
                ((Schema.V2_4.htmlentity) outputEntity).onmessage
                    = ((Schema.V2_1.htmlentity) entity).onmessage;
                ((Schema.V2_4.htmlentity) outputEntity).url
                    = ((Schema.V2_1.htmlentity) entity).url;
            }

            // Image Entity.
            else if (entity is Schema.V2_1.imageentity)
            {
                outputEntity = new Schema.V2_4.imageentity();
                ((Schema.V2_4.imageentity) outputEntity).imagefile
                    = ((Schema.V2_1.imageentity) entity).imagefile;
            }

            // Input Entity.
            else if (entity is Schema.V2_1.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V2_1.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V2_1.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V2_1.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V2_1.meshentity) entity).meshname;
            }

            // Plane Mesh Entity.
            if (entity is Schema.V2_1.planemeshentity)
            {
                outputEntity = new Schema.V2_4.planemeshentity();
                ((Schema.V2_4.planemeshentity) outputEntity).color
                    = ((Schema.V2_1.planemeshentity) entity).color;
            }

            // Prism Mesh Entity.
            if (entity is Schema.V2_1.prismmeshentity)
            {
                outputEntity = new Schema.V2_4.prismmeshentity();
                ((Schema.V2_4.prismmeshentity) outputEntity).color
                    = ((Schema.V2_1.prismmeshentity) entity).color;
            }

            // Rectangular Pyramid Mesh Entity.
            if (entity is Schema.V2_1.rectangularpyramidmeshentity)
            {
                outputEntity = new Schema.V2_4.rectangularpyramidmeshentity();
                ((Schema.V2_4.rectangularpyramidmeshentity) outputEntity).color
                    = ((Schema.V2_1.rectangularpyramidmeshentity) entity).color;
            }

            // Sphere Mesh Entity.
            if (entity is Schema.V2_1.spheremeshentity)
            {
                outputEntity = new Schema.V2_4.spheremeshentity();
                ((Schema.V2_4.spheremeshentity) outputEntity).color
                    = ((Schema.V2_1.spheremeshentity) entity).color;
            }

            // Terrain Entity.
            else if (entity is Schema.V2_1.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V2_1.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V2_1.terrainentity) entity).heights;
                List<Schema.V2_4.terrainentitylayer> outputLayers
                    = new List<Schema.V2_4.terrainentitylayer>();
                if (((Schema.V2_1.terrainentity) entity).layer != null)
                {
                    foreach (Schema.V2_1.terrainentitylayer layer
                        in ((Schema.V2_1.terrainentity) entity).layer)
                    {
                        Schema.V2_4.terrainentitylayer outputLayer
                            = new Schema.V2_4.terrainentitylayer();
                        outputLayer.metallic = layer.metallic;
                        outputLayer.normaltexture = layer.normaltexture;
                        outputLayer.masktexture = layer.masktexture;
                        outputLayer.smoothness = layer.smoothness;
                        outputLayer.diffusetexture = layer.diffusetexture;
                        outputLayer.specular = layer.specular;
                        outputLayers.Add(outputLayer);
                    }
                }
                ((Schema.V2_4.terrainentity) outputEntity).layer
                    = outputLayers.ToArray();
                ((Schema.V2_4.terrainentity) outputEntity).layermasks
                    = ((Schema.V2_1.terrainentity) entity).layermasks;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V2_1.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V2_1.terrainentity) entity).width;
                ((Schema.V2_4.terrainentity) outputEntity).type
                    = ((Schema.V2_1.terrainentity) entity).type;
            }

            // Tetrahedron Mesh Entity.
            if (entity is Schema.V2_1.tetrahedronmeshentity)
            {
                outputEntity = new Schema.V2_4.tetrahedronmeshentity();
                ((Schema.V2_4.tetrahedronmeshentity) outputEntity).color
                    = ((Schema.V2_1.tetrahedronmeshentity) entity).color;
            }

            // Text Entity.
            else if (entity is Schema.V2_1.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V2_1.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V2_1.textentity) entity).fontsize;
            }

            // Torus Mesh Entity.
            if (entity is Schema.V2_1.torusmeshentity)
            {
                outputEntity = new Schema.V2_4.torusmeshentity();
                ((Schema.V2_4.torusmeshentity) outputEntity).color
                    = ((Schema.V2_1.torusmeshentity) entity).color;
            }

            // Water Blocker Entity.
            else if (entity is Schema.V2_1.waterblockerentity)
            {
                outputEntity = new Schema.V2_4.waterblockerentity();
            }

            // Water Entity.
            else if (entity is Schema.V2_1.waterentity)
            {
                outputEntity = new Schema.V2_4.waterentity();
                ((Schema.V2_4.waterentity) outputEntity).shallowcolor
                    = ((Schema.V2_1.waterentity) entity).shallowcolor;
                ((Schema.V2_4.waterentity) outputEntity).deepcolor
                    = ((Schema.V2_1.waterentity) entity).deepcolor;
                ((Schema.V2_4.waterentity) outputEntity).specularcolor
                    = ((Schema.V2_1.waterentity) entity).specularcolor;
                ((Schema.V2_4.waterentity) outputEntity).scatteringcolor
                    = ((Schema.V2_1.waterentity) entity).scatteringcolor;
                ((Schema.V2_4.waterentity) outputEntity).deepstart
                    = ((Schema.V2_1.waterentity) entity).deepstart;
                ((Schema.V2_4.waterentity) outputEntity).deepend
                    = ((Schema.V2_1.waterentity) entity).deepend;
                ((Schema.V2_4.waterentity) outputEntity).distortion
                    = ((Schema.V2_1.waterentity) entity).distortion;
                ((Schema.V2_4.waterentity) outputEntity).smoothness
                    = ((Schema.V2_1.waterentity) entity).smoothness;
                ((Schema.V2_4.waterentity) outputEntity).numwaves
                    = ((Schema.V2_1.waterentity) entity).numwaves;
                ((Schema.V2_4.waterentity) outputEntity).waveamplitude
                    = ((Schema.V2_1.waterentity) entity).waveamplitude;
                ((Schema.V2_4.waterentity) outputEntity).wavesteepness
                    = ((Schema.V2_1.waterentity) entity).wavesteepness;
                ((Schema.V2_4.waterentity) outputEntity).wavespeed
                    = ((Schema.V2_1.waterentity) entity).wavespeed;
                ((Schema.V2_4.waterentity) outputEntity).wavelength
                    = ((Schema.V2_1.waterentity) entity).wavelength;
            }

            // Voxel Entity.
            else if (entity is Schema.V2_1.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V2_1.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V2_1.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V2_1.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V2_1.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V2_1.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V2_1.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V2_1.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V2_1.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V2_1.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V2_1.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V2_1.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V2_1.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_1.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_1.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_1.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_1.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_1.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_1.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V2_1.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V2_1.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V2_1.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V2_1.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V2_1.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V2_1.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V2_1.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V2_1.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_1.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_1.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_1.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_1.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_1.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_1.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V2_1.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V2_1.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V2_1.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            // Assign Placement Socket.
            List<Schema.V2_4.placementsocket> outputPlacementSockets = new List<Schema.V2_4.placementsocket>();
            if (entity.placementsocket != null)
            {
                foreach (Schema.V2_1.placementsocket placementSocket in entity.placementsocket)
                {
                    Schema.V2_4.placementsocket outputPlacementSocket = new Schema.V2_4.placementsocket();
                    outputPlacementSocket.position = new Schema.V2_4.position();
                    outputPlacementSocket.position.x = placementSocket.position.x;
                    outputPlacementSocket.position.y = placementSocket.position.y;
                    outputPlacementSocket.position.z = placementSocket.position.z;
                    outputPlacementSocket.rotation = new Schema.V2_4.rotation();
                    outputPlacementSocket.rotation.x = placementSocket.rotation.x;
                    outputPlacementSocket.rotation.y = placementSocket.rotation.y;
                    outputPlacementSocket.rotation.z = placementSocket.rotation.z;
                    outputPlacementSocket.rotation.w = placementSocket.rotation.w;
                    outputPlacementSocket.connectingoffset = new Schema.V2_4.position();
                    outputPlacementSocket.connectingoffset.x = placementSocket.connectingoffset.x;
                    outputPlacementSocket.connectingoffset.y = placementSocket.connectingoffset.y;
                    outputPlacementSocket.connectingoffset.z = placementSocket.connectingoffset.z;
                    outputPlacementSockets.Add(outputPlacementSocket);
                }
            }
            outputEntity.placementsocket = outputPlacementSockets.ToArray();

            return outputEntity;
        }

        /// <summary>
        /// Convert an entity from version 2.0 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV2_0(Schema.V2_0.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Arch Mesh Entity.
            if (entity is Schema.V2_0.archmeshentity)
            {
                outputEntity = new Schema.V2_4.archmeshentity();
                ((Schema.V2_4.archmeshentity) outputEntity).color
                    = ((Schema.V2_0.archmeshentity) entity).color;
            }

            // Audio Entity.
            else if (entity is Schema.V2_0.audioentity)
            {
                outputEntity = new Schema.V2_4.audioentity();
                ((Schema.V2_4.audioentity) outputEntity).audiofile
                    = ((Schema.V2_0.audioentity) entity).audiofile;
                ((Schema.V2_4.audioentity) outputEntity).autoplay
                    = ((Schema.V2_0.audioentity) entity).autoplay;
                ((Schema.V2_4.audioentity) outputEntity).loop
                    = ((Schema.V2_0.audioentity) entity).loop;
                ((Schema.V2_4.audioentity) outputEntity).priority
                    = ((Schema.V2_0.audioentity) entity).priority;
                ((Schema.V2_4.audioentity) outputEntity).volume
                    = ((Schema.V2_0.audioentity) entity).volume;
                ((Schema.V2_4.audioentity) outputEntity).pitch
                    = ((Schema.V2_0.audioentity) entity).pitch;
                ((Schema.V2_4.audioentity) outputEntity).stereopan
                    = ((Schema.V2_0.audioentity) entity).stereopan;
            }

            // Button Entity.
            else if (entity is Schema.V2_0.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V2_0.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V2_0.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Capsule Mesh Entity.
            if (entity is Schema.V2_0.capsulemeshentity)
            {
                outputEntity = new Schema.V2_4.capsulemeshentity();
                ((Schema.V2_4.capsulemeshentity) outputEntity).color
                    = ((Schema.V2_0.capsulemeshentity) entity).color;
            }

            // Character Entity.
            else if (entity is Schema.V2_0.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
                ((Schema.V2_4.characterentity) outputEntity).meshname
                    = ((Schema.V2_0.characterentity) entity).meshname;
                ((Schema.V2_4.characterentity) outputEntity).meshresource
                    = ((Schema.V2_0.characterentity) entity).meshresource;
                if (((Schema.V2_0.characterentity) entity).meshoffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.x
                        = ((Schema.V2_0.characterentity) entity).meshoffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.y
                        = ((Schema.V2_0.characterentity) entity).meshoffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.z
                        = ((Schema.V2_0.characterentity) entity).meshoffset.z;
                }
                if (((Schema.V2_0.characterentity) entity).meshrotation != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation
                        = new Schema.V2_4.rotation();
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.x
                        = ((Schema.V2_0.characterentity) entity).meshrotation.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.y
                        = ((Schema.V2_0.characterentity) entity).meshrotation.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.z
                        = ((Schema.V2_0.characterentity) entity).meshrotation.z;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.w
                        = ((Schema.V2_0.characterentity) entity).meshrotation.w;
                }
                if (((Schema.V2_0.characterentity) entity).labeloffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.x
                        = ((Schema.V2_0.characterentity) entity).labeloffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.y
                        = ((Schema.V2_0.characterentity) entity).labeloffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.z
                        = ((Schema.V2_0.characterentity) entity).labeloffset.z;
                }
            }

            // Cone Mesh Entity.
            if (entity is Schema.V2_0.conemeshentity)
            {
                outputEntity = new Schema.V2_4.conemeshentity();
                ((Schema.V2_4.conemeshentity) outputEntity).color
                    = ((Schema.V2_0.conemeshentity) entity).color;
            }

            // Container Entity.
            else if (entity is Schema.V2_0.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // Cube Mesh Entity.
            if (entity is Schema.V2_0.cubemeshentity)
            {
                outputEntity = new Schema.V2_4.cubemeshentity();
                ((Schema.V2_4.cubemeshentity) outputEntity).color
                    = ((Schema.V2_0.cubemeshentity) entity).color;
            }

            // Cylinder Mesh Entity.
            if (entity is Schema.V2_0.cylindermeshentity)
            {
                outputEntity = new Schema.V2_4.cylindermeshentity();
                ((Schema.V2_4.cylindermeshentity) outputEntity).color
                    = ((Schema.V2_0.cylindermeshentity) entity).color;
            }

            // HTML Entity.
            else if (entity is Schema.V2_0.htmlentity)
            {
                outputEntity = new Schema.V2_4.htmlentity();
                ((Schema.V2_4.htmlentity) outputEntity).onmessage
                    = ((Schema.V2_0.htmlentity) entity).onmessage;
                ((Schema.V2_4.htmlentity) outputEntity).url
                    = ((Schema.V2_0.htmlentity) entity).url;
            }

            // Image Entity.
            else if (entity is Schema.V2_0.imageentity)
            {
                outputEntity = new Schema.V2_4.imageentity();
                ((Schema.V2_4.imageentity) outputEntity).imagefile
                    = ((Schema.V2_0.imageentity) entity).imagefile;
            }

            // Input Entity.
            else if (entity is Schema.V2_0.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V2_0.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V2_0.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V2_0.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V2_0.meshentity) entity).meshname;
            }

            // Plane Mesh Entity.
            if (entity is Schema.V2_0.planemeshentity)
            {
                outputEntity = new Schema.V2_4.planemeshentity();
                ((Schema.V2_4.planemeshentity) outputEntity).color
                    = ((Schema.V2_0.planemeshentity) entity).color;
            }

            // Prism Mesh Entity.
            if (entity is Schema.V2_0.prismmeshentity)
            {
                outputEntity = new Schema.V2_4.prismmeshentity();
                ((Schema.V2_4.prismmeshentity) outputEntity).color
                    = ((Schema.V2_0.prismmeshentity) entity).color;
            }

            // Rectangular Pyramid Mesh Entity.
            if (entity is Schema.V2_0.rectangularpyramidmeshentity)
            {
                outputEntity = new Schema.V2_4.rectangularpyramidmeshentity();
                ((Schema.V2_4.rectangularpyramidmeshentity) outputEntity).color
                    = ((Schema.V2_0.rectangularpyramidmeshentity) entity).color;
            }

            // Sphere Mesh Entity.
            if (entity is Schema.V2_0.spheremeshentity)
            {
                outputEntity = new Schema.V2_4.spheremeshentity();
                ((Schema.V2_4.spheremeshentity) outputEntity).color
                    = ((Schema.V2_0.spheremeshentity) entity).color;
            }

            // Terrain Entity.
            else if (entity is Schema.V2_0.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V2_0.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V2_0.terrainentity) entity).heights;
                List<Schema.V2_4.terrainentitylayer> outputLayers
                    = new List<Schema.V2_4.terrainentitylayer>();
                if (((Schema.V2_0.terrainentity) entity).layer != null)
                {
                    foreach (Schema.V2_0.terrainentitylayer layer
                        in ((Schema.V2_0.terrainentity) entity).layer)
                    {
                        Schema.V2_4.terrainentitylayer outputLayer
                            = new Schema.V2_4.terrainentitylayer();
                        outputLayer.metallic = layer.metallic;
                        outputLayer.normaltexture = layer.normaltexture;
                        outputLayer.masktexture = layer.masktexture;
                        outputLayer.smoothness = layer.smoothness;
                        outputLayer.diffusetexture = layer.diffusetexture;
                        outputLayer.specular = layer.specular;
                        outputLayers.Add(outputLayer);
                    }
                }
                ((Schema.V2_4.terrainentity) outputEntity).layer
                    = outputLayers.ToArray();
                ((Schema.V2_4.terrainentity) outputEntity).layermasks
                    = ((Schema.V2_0.terrainentity) entity).layermasks;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V2_0.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V2_0.terrainentity) entity).width;
                ((Schema.V2_4.terrainentity) outputEntity).type
                    = ((Schema.V2_0.terrainentity) entity).type;
            }

            // Tetrahedron Mesh Entity.
            if (entity is Schema.V2_0.tetrahedronmeshentity)
            {
                outputEntity = new Schema.V2_4.tetrahedronmeshentity();
                ((Schema.V2_4.tetrahedronmeshentity) outputEntity).color
                    = ((Schema.V2_0.tetrahedronmeshentity) entity).color;
            }

            // Text Entity.
            else if (entity is Schema.V2_0.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V2_0.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V2_0.textentity) entity).fontsize;
            }

            // Torus Mesh Entity.
            if (entity is Schema.V2_0.torusmeshentity)
            {
                outputEntity = new Schema.V2_4.torusmeshentity();
                ((Schema.V2_4.torusmeshentity) outputEntity).color
                    = ((Schema.V2_0.torusmeshentity) entity).color;
            }

            // Voxel Entity.
            else if (entity is Schema.V2_0.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V2_0.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V2_0.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V2_0.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V2_0.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V2_0.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V2_0.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V2_0.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V2_0.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V2_0.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V2_0.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V2_0.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V2_0.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_0.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_0.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_0.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_0.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_0.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_0.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V2_0.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V2_0.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V2_0.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V2_0.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V2_0.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V2_0.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V2_0.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V2_0.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V2_0.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V2_0.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V2_0.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V2_0.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V2_0.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V2_0.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V2_0.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V2_0.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V2_0.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            // Assign Placement Socket.
            List<Schema.V2_4.placementsocket> outputPlacementSockets = new List<Schema.V2_4.placementsocket>();
            if (entity.placementsocket != null)
            {
                foreach (Schema.V2_0.placementsocket placementSocket in entity.placementsocket)
                {
                    Schema.V2_4.placementsocket outputPlacementSocket = new Schema.V2_4.placementsocket();
                    outputPlacementSocket.position = new Schema.V2_4.position();
                    outputPlacementSocket.position.x = placementSocket.position.x;
                    outputPlacementSocket.position.y = placementSocket.position.y;
                    outputPlacementSocket.position.z = placementSocket.position.z;
                    outputPlacementSocket.rotation = new Schema.V2_4.rotation();
                    outputPlacementSocket.rotation.x = placementSocket.rotation.x;
                    outputPlacementSocket.rotation.y = placementSocket.rotation.y;
                    outputPlacementSocket.rotation.z = placementSocket.rotation.z;
                    outputPlacementSocket.rotation.w = placementSocket.rotation.w;
                    outputPlacementSocket.connectingoffset = new Schema.V2_4.position();
                    outputPlacementSocket.connectingoffset.x = placementSocket.connectingoffset.x;
                    outputPlacementSocket.connectingoffset.y = placementSocket.connectingoffset.y;
                    outputPlacementSocket.connectingoffset.z = placementSocket.connectingoffset.z;
                    outputPlacementSockets.Add(outputPlacementSocket);
                }
            }
            outputEntity.placementsocket = outputPlacementSockets.ToArray();

            return outputEntity;
        }

        /// <summary>
        /// Convert an entity from version 1.3 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV1_3(Schema.V1_3.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Button Entity.
            if (entity is Schema.V1_3.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V1_3.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V1_3.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Character Entity.
            else if (entity is Schema.V1_3.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
                ((Schema.V2_4.characterentity) outputEntity).meshname
                    = ((Schema.V1_3.characterentity) entity).meshname;
                ((Schema.V2_4.characterentity) outputEntity).meshresource
                    = ((Schema.V1_3.characterentity) entity).meshresource;
                if (((Schema.V1_3.characterentity) entity).meshoffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.x
                        = ((Schema.V1_3.characterentity) entity).meshoffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.y
                        = ((Schema.V1_3.characterentity) entity).meshoffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshoffset.z
                        = ((Schema.V1_3.characterentity) entity).meshoffset.z;
                }
                if (((Schema.V1_3.characterentity) entity).meshrotation != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation
                        = new Schema.V2_4.rotation();
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.x
                        = ((Schema.V1_3.characterentity) entity).meshrotation.x;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.y
                        = ((Schema.V1_3.characterentity) entity).meshrotation.y;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.z
                        = ((Schema.V1_3.characterentity) entity).meshrotation.z;
                    ((Schema.V2_4.characterentity) outputEntity).meshrotation.w
                        = ((Schema.V1_3.characterentity) entity).meshrotation.w;
                }
                if (((Schema.V1_3.characterentity) entity).labeloffset != null)
                {
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset
                        = new Schema.V2_4.position();
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.x
                        = ((Schema.V1_3.characterentity) entity).labeloffset.x;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.y
                        = ((Schema.V1_3.characterentity) entity).labeloffset.y;
                    ((Schema.V2_4.characterentity) outputEntity).labeloffset.z
                        = ((Schema.V1_3.characterentity) entity).labeloffset.z;
                }
            }

            // Container Entity.
            else if (entity is Schema.V1_3.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // HTML Entity.
            else if (entity is Schema.V1_3.htmlentity)
            {
                outputEntity = new Schema.V2_4.htmlentity();
                ((Schema.V2_4.htmlentity) outputEntity).onmessage
                    = ((Schema.V1_3.htmlentity) entity).onmessage;
                ((Schema.V2_4.htmlentity) outputEntity).url
                    = ((Schema.V1_3.htmlentity) entity).url;
            }

            // Input Entity.
            else if (entity is Schema.V1_3.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V1_3.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V1_3.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V1_3.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V1_3.meshentity) entity).meshname;
            }

            // Terrain Entity.
            else if (entity is Schema.V1_3.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V1_3.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V1_3.terrainentity) entity).heights;
                List<Schema.V2_4.terrainentitylayer> outputLayers
                    = new List<Schema.V2_4.terrainentitylayer>();
                if (((Schema.V1_3.terrainentity) entity).layer != null)
                {
                    foreach (Schema.V1_3.terrainentitylayer layer
                        in ((Schema.V1_3.terrainentity) entity).layer)
                    {
                        Schema.V2_4.terrainentitylayer outputLayer
                            = new Schema.V2_4.terrainentitylayer();
                        outputLayer.metallic = layer.metallic;
                        outputLayer.normaltexture = layer.normaltexture;
                        outputLayer.masktexture = layer.masktexture;
                        outputLayer.smoothness = layer.smoothness;
                        outputLayer.diffusetexture = layer.diffusetexture;
                        outputLayer.specular = layer.specular;
                        outputLayers.Add(outputLayer);
                    }
                }
                ((Schema.V2_4.terrainentity) outputEntity).layer
                    = outputLayers.ToArray();
                ((Schema.V2_4.terrainentity) outputEntity).layermasks
                    = ((Schema.V1_3.terrainentity) entity).layermasks;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V1_3.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V1_3.terrainentity) entity).width;
                ((Schema.V2_4.terrainentity) outputEntity).type
                    = ((Schema.V1_3.terrainentity) entity).type;
            }

            // Text Entity.
            else if (entity is Schema.V1_3.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V1_3.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V1_3.textentity) entity).fontsize;
            }

            // Voxel Entity.
            else if (entity is Schema.V1_3.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V1_3.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V1_3.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V1_3.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V1_3.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V1_3.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V1_3.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V1_3.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V1_3.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V1_3.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V1_3.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V1_3.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V1_3.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_3.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_3.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_3.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_3.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_3.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_3.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V1_3.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V1_3.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V1_3.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V1_3.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V1_3.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V1_3.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V1_3.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V1_3.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_3.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_3.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_3.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_3.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_3.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_3.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V1_3.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V1_3.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V1_3.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            // Assign Placement Socket.
            List<Schema.V2_4.placementsocket> outputPlacementSockets = new List<Schema.V2_4.placementsocket>();
            if (entity.placementsocket != null)
            {
                foreach (Schema.V1_3.placementsocket placementSocket in entity.placementsocket)
                {
                    Schema.V2_4.placementsocket outputPlacementSocket = new Schema.V2_4.placementsocket();
                    outputPlacementSocket.position = new Schema.V2_4.position();
                    outputPlacementSocket.position.x = placementSocket.position.x;
                    outputPlacementSocket.position.y = placementSocket.position.y;
                    outputPlacementSocket.position.z = placementSocket.position.z;
                    outputPlacementSocket.rotation = new Schema.V2_4.rotation();
                    outputPlacementSocket.rotation.x = placementSocket.rotation.x;
                    outputPlacementSocket.rotation.y = placementSocket.rotation.y;
                    outputPlacementSocket.rotation.z = placementSocket.rotation.z;
                    outputPlacementSocket.rotation.w = placementSocket.rotation.w;
                    outputPlacementSocket.connectingoffset = new Schema.V2_4.position();
                    outputPlacementSocket.connectingoffset.x = placementSocket.connectingoffset.x;
                    outputPlacementSocket.connectingoffset.y = placementSocket.connectingoffset.y;
                    outputPlacementSocket.connectingoffset.z = placementSocket.connectingoffset.z;
                    outputPlacementSockets.Add(outputPlacementSocket);
                }
            }
            outputEntity.placementsocket = outputPlacementSockets.ToArray();

            return outputEntity;
        }

        /// <summary>
        /// Convert an entity from version 1.2 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV1_2(Schema.V1_2.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Button Entity.
            if (entity is Schema.V1_2.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V1_2.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V1_2.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Character Entity.
            else if (entity is Schema.V1_2.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
            }

            // Container Entity.
            else if (entity is Schema.V1_2.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // HTML Entity.
            else if (entity is Schema.V1_2.htmlentity)
            {
                outputEntity = new Schema.V2_4.htmlentity();
                ((Schema.V2_4.htmlentity) outputEntity).onmessage
                    = ((Schema.V1_2.htmlentity) entity).onmessage;
                ((Schema.V2_4.htmlentity) outputEntity).url
                    = ((Schema.V1_2.htmlentity) entity).url;
            }

            // Input Entity.
            else if (entity is Schema.V1_2.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V1_2.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V1_2.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V1_2.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V1_2.meshentity) entity).meshname;
            }

            // Terrain Entity.
            else if (entity is Schema.V1_2.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V1_2.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V1_2.terrainentity) entity).heights;
                List<Schema.V2_4.terrainentitylayer> outputLayers
                    = new List<Schema.V2_4.terrainentitylayer>();
                if (((Schema.V1_2.terrainentity) entity).layer != null)
                {
                    foreach (Schema.V1_2.terrainentitylayer layer
                        in ((Schema.V1_2.terrainentity) entity).layer)
                    {
                        Schema.V2_4.terrainentitylayer outputLayer
                            = new Schema.V2_4.terrainentitylayer();
                        outputLayer.metallic = layer.metallic;
                        outputLayer.normaltexture = layer.normaltexture;
                        outputLayer.masktexture = layer.masktexture;
                        outputLayer.smoothness = layer.smoothness;
                        outputLayer.diffusetexture = layer.diffusetexture;
                        outputLayer.specular = layer.specular;
                        outputLayers.Add(outputLayer);
                    }
                }
                ((Schema.V2_4.terrainentity) outputEntity).layer
                    = outputLayers.ToArray();
                ((Schema.V2_4.terrainentity) outputEntity).layermasks
                    = ((Schema.V1_2.terrainentity) entity).layermasks;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V1_2.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V1_2.terrainentity) entity).width;
                ((Schema.V2_4.terrainentity) outputEntity).type
                    = ((Schema.V1_2.terrainentity) entity).type;
            }

            // Text Entity.
            else if (entity is Schema.V1_2.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V1_2.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V1_2.textentity) entity).fontsize;
            }

            // Voxel Entity.
            else if (entity is Schema.V1_2.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V1_2.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V1_2.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V1_2.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V1_2.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V1_2.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V1_2.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V1_2.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V1_2.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V1_2.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V1_2.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V1_2.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V1_2.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_2.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_2.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_2.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_2.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_2.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_2.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V1_2.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V1_2.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V1_2.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V1_2.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V1_2.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V1_2.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V1_2.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V1_2.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_2.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_2.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_2.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_2.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_2.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_2.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V1_2.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V1_2.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V1_2.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            // Assign Placement Socket.
            List<Schema.V2_4.placementsocket> outputPlacementSockets = new List<Schema.V2_4.placementsocket>();
            if (entity.placementsocket != null)
            {
                foreach (Schema.V1_2.placementsocket placementSocket in entity.placementsocket)
                {
                    Schema.V2_4.placementsocket outputPlacementSocket = new Schema.V2_4.placementsocket();
                    outputPlacementSocket.position = new Schema.V2_4.position();
                    outputPlacementSocket.position.x = placementSocket.position.x;
                    outputPlacementSocket.position.y = placementSocket.position.y;
                    outputPlacementSocket.position.z = placementSocket.position.z;
                    outputPlacementSocket.rotation = new Schema.V2_4.rotation();
                    outputPlacementSocket.rotation.x = placementSocket.rotation.x;
                    outputPlacementSocket.rotation.y = placementSocket.rotation.y;
                    outputPlacementSocket.rotation.z = placementSocket.rotation.z;
                    outputPlacementSocket.rotation.w = placementSocket.rotation.w;
                    outputPlacementSocket.connectingoffset = new Schema.V2_4.position();
                    outputPlacementSocket.connectingoffset.x = placementSocket.connectingoffset.x;
                    outputPlacementSocket.connectingoffset.y = placementSocket.connectingoffset.y;
                    outputPlacementSocket.connectingoffset.z = placementSocket.connectingoffset.z;
                    outputPlacementSockets.Add(outputPlacementSocket);
                }
            }
            outputEntity.placementsocket = outputPlacementSockets.ToArray();

            return outputEntity;
        }

        /// <summary>
        /// Convert an entity from version 1.1 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV1_1(Schema.V1_1.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Button Entity.
            if (entity is Schema.V1_1.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V1_1.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V1_1.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Character Entity.
            else if (entity is Schema.V1_1.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
            }

            // Container Entity.
            else if (entity is Schema.V1_1.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // Input Entity.
            else if (entity is Schema.V1_1.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V1_1.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V1_1.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V1_1.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V1_1.meshentity) entity).meshname;
            }

            // Terrain Entity.
            else if (entity is Schema.V1_1.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V1_1.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V1_1.terrainentity) entity).heights;
                ((Schema.V2_4.terrainentity) outputEntity).layer = null;
                ((Schema.V2_4.terrainentity) outputEntity).layermasks = null;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V1_1.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V1_1.terrainentity) entity).width;
            }

            // Text Entity.
            else if (entity is Schema.V1_1.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V1_1.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V1_1.textentity) entity).fontsize;
            }

            // Voxel Entity.
            else if (entity is Schema.V1_1.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V1_1.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V1_1.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V1_1.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V1_1.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V1_1.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V1_1.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V1_1.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V1_1.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V1_1.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V1_1.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V1_1.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V1_1.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_1.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_1.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_1.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_1.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_1.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_1.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V1_1.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V1_1.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V1_1.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V1_1.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V1_1.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V1_1.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V1_1.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V1_1.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_1.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_1.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_1.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_1.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_1.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_1.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V1_1.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V1_1.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V1_1.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            // Assign Placement Socket.
            List<Schema.V2_4.placementsocket> outputPlacementSockets = new List<Schema.V2_4.placementsocket>();
            if (entity.placementsocket != null)
            {
                foreach (Schema.V1_1.placementsocket placementSocket in entity.placementsocket)
                {
                    Schema.V2_4.placementsocket outputPlacementSocket = new Schema.V2_4.placementsocket();
                    outputPlacementSocket.position = new Schema.V2_4.position();
                    outputPlacementSocket.position.x = placementSocket.position.x;
                    outputPlacementSocket.position.y = placementSocket.position.y;
                    outputPlacementSocket.position.z = placementSocket.position.z;
                    outputPlacementSocket.rotation = new Schema.V2_4.rotation();
                    outputPlacementSocket.rotation.x = placementSocket.rotation.x;
                    outputPlacementSocket.rotation.y = placementSocket.rotation.y;
                    outputPlacementSocket.rotation.z = placementSocket.rotation.z;
                    outputPlacementSocket.rotation.w = placementSocket.rotation.w;
                    outputPlacementSocket.connectingoffset = new Schema.V2_4.position();
                    outputPlacementSocket.connectingoffset.x = placementSocket.connectingoffset.x;
                    outputPlacementSocket.connectingoffset.y = placementSocket.connectingoffset.y;
                    outputPlacementSocket.connectingoffset.z = placementSocket.connectingoffset.z;
                    outputPlacementSockets.Add(outputPlacementSocket);
                }
            }
            outputEntity.placementsocket = outputPlacementSockets.ToArray();

            return outputEntity;
        }

        /// <summary>
        /// Convert an entity from version 1.0 to the current schema (version 2.4).
        /// </summary>
        /// <param name="entity">Input entity instance.</param>
        /// <returns>Current schema version for the input entity instance.</returns>
        private static Schema.V2_4.entity ConvertEntityFromV1_0(Schema.V1_0.entity entity)
        {
            // Assign entity.
            Schema.V2_4.entity outputEntity;

            // Button Entity.
            if (entity is Schema.V1_0.buttonentity)
            {
                outputEntity = new Schema.V2_4.buttonentity();
                ((Schema.V2_4.buttonentity) outputEntity).onclickevent
                    = ((Schema.V1_0.buttonentity) entity).onclickevent;
            }

            // Canvas Entity.
            else if (entity is Schema.V1_0.canvasentity)
            {
                outputEntity = new Schema.V2_4.canvasentity();
            }

            // Character Entity.
            else if (entity is Schema.V1_0.characterentity)
            {
                outputEntity = new Schema.V2_4.characterentity();
            }

            // Container Entity.
            else if (entity is Schema.V1_0.containerentity)
            {
                outputEntity = new Schema.V2_4.containerentity();
            }

            // Input Entity.
            else if (entity is Schema.V1_0.inputentity)
            {
                outputEntity = new Schema.V2_4.inputentity();
            }

            // LightEntity.
            else if (entity is Schema.V1_0.lightentity)
            {
                outputEntity = new Schema.V2_4.lightentity();
            }

            // Mesh Entity.
            else if (entity is Schema.V1_0.meshentity)
            {
                outputEntity = new Schema.V2_4.meshentity();
                ((Schema.V2_4.meshentity) outputEntity).meshresource
                    = ((Schema.V1_0.meshentity) entity).meshresource;
                ((Schema.V2_4.meshentity) outputEntity).meshname
                    = ((Schema.V1_0.meshentity) entity).meshname;
            }

            // Terrain Entity.
            else if (entity is Schema.V1_0.terrainentity)
            {
                outputEntity = new Schema.V2_4.terrainentity();
                ((Schema.V2_4.terrainentity) outputEntity).height
                    = ((Schema.V1_0.terrainentity) entity).height;
                ((Schema.V2_4.terrainentity) outputEntity).heights
                    = ((Schema.V1_0.terrainentity) entity).heights;
                ((Schema.V2_4.terrainentity) outputEntity).layer = null;
                ((Schema.V2_4.terrainentity) outputEntity).layermasks = null;
                ((Schema.V2_4.terrainentity) outputEntity).length
                    = ((Schema.V1_0.terrainentity) entity).length;
                ((Schema.V2_4.terrainentity) outputEntity).width
                    = ((Schema.V1_0.terrainentity) entity).width;
            }

            // Text Entity.
            else if (entity is Schema.V1_0.textentity)
            {
                outputEntity = new Schema.V2_4.textentity();
                ((Schema.V2_4.textentity) outputEntity).text
                    = ((Schema.V1_0.textentity) entity).text;
                ((Schema.V2_4.textentity) outputEntity).fontsize
                    = ((Schema.V1_0.textentity) entity).fontsize;
            }

            // Voxel Entity.
            else if (entity is Schema.V1_0.voxelentity)
            {
                outputEntity = new Schema.V2_4.voxelentity();
            }

            // Default to Base Entity.
            else
            {
                outputEntity = new Schema.V2_4.entity();
            }

            // Assign id.
            outputEntity.id = entity.id;

            // Assign tag.
            outputEntity.tag = entity.tag;

            // Assign transform.
            if (entity.transform != null)
            {
                // Canvas Transform.
                if (entity.transform is Schema.V1_0.canvastransform)
                {
                    // Assign Canvas Transform.
                    outputEntity.transform = new Schema.V2_4.canvastransform();

                    // Assign Position Percent.
                    if (((Schema.V1_0.canvastransform) entity.transform).positionpercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent =
                            new Schema.V2_4.positionpercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.x =
                            ((Schema.V1_0.canvastransform) entity.transform).positionpercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).positionpercent.y =
                            ((Schema.V1_0.canvastransform) entity.transform).positionpercent.y;
                    }

                    // Assign Size Percent.
                    if (((Schema.V1_0.canvastransform) entity.transform).sizepercent != null)
                    {
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent =
                            new Schema.V2_4.sizepercent();
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.x =
                            ((Schema.V1_0.canvastransform) entity.transform).sizepercent.x;
                        ((Schema.V2_4.canvastransform) outputEntity.transform).sizepercent.y =
                            ((Schema.V1_0.canvastransform) entity.transform).sizepercent.y;
                    }
                }

                // Scale Transform.
                else if (entity.transform is Schema.V1_0.scaletransform)
                {
                    // Assign Scale Transform.
                    outputEntity.transform = new Schema.V2_4.scaletransform();

                    // Assign Position.
                    if (((Schema.V1_0.scaletransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.x =
                            ((Schema.V1_0.scaletransform) entity.transform).position.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.y =
                            ((Schema.V1_0.scaletransform) entity.transform).position.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).position.z =
                            ((Schema.V1_0.scaletransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_0.scaletransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_0.scaletransform) entity.transform).rotation.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_0.scaletransform) entity.transform).rotation.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_0.scaletransform) entity.transform).rotation.z;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_0.scaletransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_0.scaletransform) entity.transform).scale != null)
                    {
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale =
                            new Schema.V2_4.scale();
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.x =
                            ((Schema.V1_0.scaletransform) entity.transform).scale.x;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.y =
                            ((Schema.V1_0.scaletransform) entity.transform).scale.y;
                        ((Schema.V2_4.scaletransform) outputEntity.transform).scale.z =
                            ((Schema.V1_0.scaletransform) entity.transform).scale.z;
                    }
                }

                // Size Transform.
                else if (entity.transform is Schema.V1_0.sizetransform)
                {
                    // Assign Size Transform.
                    outputEntity.transform = new Schema.V2_4.sizetransform();

                    // Assign Position.
                    if (((Schema.V1_0.sizetransform) entity.transform).position != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position =
                            new Schema.V2_4.position();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.x =
                            ((Schema.V1_0.sizetransform) entity.transform).position.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.y =
                            ((Schema.V1_0.sizetransform) entity.transform).position.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).position.z =
                            ((Schema.V1_0.sizetransform) entity.transform).position.z;
                    }

                    // Assign Rotation.
                    if (((Schema.V1_0.sizetransform) entity.transform).rotation != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation =
                            new Schema.V2_4.rotation();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.x =
                            ((Schema.V1_0.sizetransform) entity.transform).rotation.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.y =
                            ((Schema.V1_0.sizetransform) entity.transform).rotation.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.z =
                            ((Schema.V1_0.sizetransform) entity.transform).rotation.z;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).rotation.w =
                            ((Schema.V1_0.sizetransform) entity.transform).rotation.w;
                    }

                    // Assign Scale.
                    if (((Schema.V1_0.sizetransform) entity.transform).size != null)
                    {
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size =
                            new Schema.V2_4.size();
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.x =
                            ((Schema.V1_0.sizetransform) entity.transform).size.x;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.y =
                            ((Schema.V1_0.sizetransform) entity.transform).size.y;
                        ((Schema.V2_4.sizetransform) outputEntity.transform).size.z =
                            ((Schema.V1_0.sizetransform) entity.transform).size.z;
                    }
                }

                // Default to Base Transform.
                else
                {
                    outputEntity.transform = new Schema.V2_4.basetransform();
                }
            }

            // Assign On Load Event.
            outputEntity.onloadevent = entity.onloadevent;

            // Assign Synchronizer.
            outputEntity.synchronizer = entity.synchronizer;

            return outputEntity;
        }

        /// <summary>
        /// Add an entity to a Version 2.1 entity array.
        /// </summary>
        /// <param name="baseArray">Initial array.</param>
        /// <param name="entityToAdd">Entity to add to array.</param>
        /// <returns>The input array with the specified entity added.</returns>
        private static Schema.V2_4.entity[] AddToV2_4EntityArray(Schema.V2_4.entity[] baseArray,
            Schema.V2_4.entity entityToAdd)
        {
            List<Schema.V2_4.entity> entityList;
            if (baseArray == null)
            {
                entityList = new List<Schema.V2_4.entity>();
            }
            else
            {
                entityList = new List<Schema.V2_4.entity>(baseArray);
            }
            entityList.Add(entityToAdd);
            return entityList.ToArray();
        }
    }
}