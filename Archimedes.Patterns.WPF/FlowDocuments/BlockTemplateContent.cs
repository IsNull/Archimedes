﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Archimedes.Patterns.WPF.FlowDocuments
{
    public class BlockTemplateContent : Section
    {
        private static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(DataTemplate), typeof(BlockTemplateContent), new PropertyMetadata(OnTemplateChanged));

        public DataTemplate Template {
            get {
                return (DataTemplate)GetValue(TemplateProperty);
            }
            set {
                SetValue(TemplateProperty, value);
            }
        }


        public BlockTemplateContent() {
            DocHelpers.FixupDataContext(this);
            Loaded += BlockTemplateContent_Loaded;
        }


        private void BlockTemplateContent_Loaded(object sender, RoutedEventArgs e) {
            GenerateContent(Template);
        }


        private void GenerateContent(DataTemplate template) {
            Blocks.Clear();
            if (template != null) {
                FrameworkContentElement element = DocHelpers.LoadDataTemplate(template);
                Blocks.Add((Block)element);
            }
        }

        private void OnTemplateChanged(DataTemplate dataTemplate) {
            GenerateContent(dataTemplate);
        }


        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((BlockTemplateContent)d).OnTemplateChanged((DataTemplate)e.NewValue);
        }
    }
}
