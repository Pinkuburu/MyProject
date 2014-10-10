/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2012 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

using System;
using System.Collections.Generic;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareMonitor.GUI
{
    public class TypeNode : Node
    {

        private SensorType sensorType;

        public TypeNode(SensorType sensorType) : base()
        {
            this.sensorType = sensorType;

            switch (sensorType)
            {
                case SensorType.Voltage:
                    this.Image = Utilities.EmbeddedResources.GetImage("voltage.png");
                    this.Text = "电压";
                    break;
                case SensorType.Clock:
                    this.Image = Utilities.EmbeddedResources.GetImage("clock.png");
                    this.Text = "时钟";
                    break;
                case SensorType.Load:
                    this.Image = Utilities.EmbeddedResources.GetImage("load.png");
                    this.Text = "负荷";
                    break;
                case SensorType.Temperature:
                    this.Image = Utilities.EmbeddedResources.GetImage("temperature.png");
                    this.Text = "温度";
                    break;
                case SensorType.Fan:
                    this.Image = Utilities.EmbeddedResources.GetImage("fan.png");
                    this.Text = "风扇";
                    break;
                case SensorType.Flow:
                    this.Image = Utilities.EmbeddedResources.GetImage("flow.png");
                    this.Text = "流量";
                    break;
                case SensorType.Control:
                    this.Image = Utilities.EmbeddedResources.GetImage("control.png");
                    this.Text = "控制";
                    break;
                case SensorType.Level:
                    this.Image = Utilities.EmbeddedResources.GetImage("level.png");
                    this.Text = "级别";
                    break;
                case SensorType.Power:
                    this.Image = Utilities.EmbeddedResources.GetImage("power.png");
                    this.Text = "功率";
                    break;
                case SensorType.Data:
                    this.Image = Utilities.EmbeddedResources.GetImage("data.png");
                    this.Text = "数据";
                    break;
                case SensorType.Factor:
                    this.Image = Utilities.EmbeddedResources.GetImage("factor.png");
                    this.Text = "因素";
                    break;
            }

            NodeAdded += new NodeEventHandler(TypeNode_NodeAdded);
            NodeRemoved += new NodeEventHandler(TypeNode_NodeRemoved);
        }

        private void TypeNode_NodeRemoved(Node node)
        {
            node.IsVisibleChanged -= new NodeEventHandler(node_IsVisibleChanged);
            node_IsVisibleChanged(null);
        }

        private void TypeNode_NodeAdded(Node node)
        {
            node.IsVisibleChanged += new NodeEventHandler(node_IsVisibleChanged);
            node_IsVisibleChanged(null);
        }

        private void node_IsVisibleChanged(Node node)
        {
            foreach (Node n in Nodes)
                if (n.IsVisible)
                {
                    this.IsVisible = true;
                    return;
                }
            this.IsVisible = false;
        }

        public SensorType SensorType
        {
            get { return sensorType; }
        }
    }
}
