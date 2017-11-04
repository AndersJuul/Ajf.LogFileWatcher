using System;
using System.Linq;
using LogFileWatcher.Service;
using NUnit.Framework;

namespace Ajf.LogFileWatcher.Tests
{
    [TestFixture]
    public class AppSettingsProviderTests
    {
        [Test]
        public void TestThatAppSettingsProviderSplitsMonitoringTargetsCorrectly()
        {
            // Arrange
            var sut = new AppSettingsProvider();

            // Act
            var monitoringTargets = sut.GetMonitoringTargets(@"c:\abc.log;1;0|c:\def.log;1;2");

            // Assert
            Assert.That(monitoringTargets.Count(), Is.EqualTo(2));

            Assert.That(monitoringTargets.ToArray()[0].Path, Is.EqualTo(@"c:\abc.log"));
            Assert.That(monitoringTargets.ToArray()[0].MaxAge, Is.EqualTo(new TimeSpan(0, 1, 0)));

            Assert.That(monitoringTargets.ToArray()[1].Path, Is.EqualTo(@"c:\def.log"));
            Assert.That(monitoringTargets.ToArray()[1].MaxAge, Is.EqualTo(new TimeSpan(0, 1, 2)));
        }

        [Test]
        public void TestThatAppSettingsProviderSplitsMonitoringTargetCorrectly()
        {
            // Arrange
            var sut = new AppSettingsProvider();

            // Act
            var monitoringTargets = sut.GetMonitoringTargets(@"c:\abc.log;1;0");

            // Assert
            Assert.That(monitoringTargets.Count(), Is.EqualTo(1));
            Assert.That(monitoringTargets.ToArray()[0].Path, Is.EqualTo(@"c:\abc.log"));
            Assert.That(monitoringTargets.ToArray()[0].MaxAge, Is.EqualTo(new TimeSpan(0, 1, 0)));
        }
    }
}
