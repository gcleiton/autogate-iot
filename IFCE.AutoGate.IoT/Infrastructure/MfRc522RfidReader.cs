using IFCE.AutoGate.IoT.Constants;
using IFCE.AutoGate.IoT.Models;
using Iot.Device.Card.Mifare;
using Iot.Device.Mfrc522;
using Iot.Device.Rfid;
using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;
using Esp32 = nanoFramework.Hardware.Esp32;

namespace IFCE.AutoGate.IoT.Infrastructure
{
    class MfRc522RfidReader
    {
        private readonly MfRc522 mfrc522;
        public MfRc522RfidReader(GpioController gpioController)
        { 
            SetupEsp32Configuration();

            var spiDevice = CreateSpiDevice();
            mfrc522 = new MfRc522(spiDevice, (int)MfRc522RfidReaderPinout.RST, gpioController, false);
        }

        public int WaitCard()
        {
            Data106kbpsTypeA card;
            WaitDetectCard(out card);

            var mifareCard = SetupMifareCard(card);

            mifareCard.Command = MifareCardCommand.AuthenticationB;
            var result = mifareCard.RunMifareCardCommand();
            if (result < 0)
            {
                mifareCard.ReselectCard();
                throw new RfidException();
            }

            return ReadCardNumber(mifareCard);
        }

        private void WaitDetectCard(out Data106kbpsTypeA card)
        {
            Debug.WriteLine("Awaiting RFID card...");
            bool wasDetected;
            do
            {

                wasDetected = mfrc522.ListenToCardIso14443TypeA(out card, TimeSpan.FromSeconds(2));
                Thread.Sleep(wasDetected ? 0 : 500);
            }
            while (!wasDetected);
            Debug.WriteLine("RFID card detected!");
        }

        private MifareCard SetupMifareCard(Data106kbpsTypeA card)
        {
            return new MifareCard(mfrc522, card.TargetNumber)
            {
                SerialNumber = card.NfcId,
                Capacity = MifareCardCapacity.Mifare1K,
                KeyA = MifareCard.DefaultKeyA.ToArray(),
                KeyB = MifareCard.DefaultKeyB.ToArray(),
                BlockNumber = 0,
            };
        }

        private int ReadCardNumber(MifareCard card)
        {
            card.Command = MifareCardCommand.Read16Bytes;
            var result = card.RunMifareCardCommand();
            if (result >= 0 && card.Data is not null)
            {
                var cardNumber = BitConverter.ToInt32(card.Data, 0);
                Debug.WriteLine($"Card with number {cardNumber} successfully read.");
                return cardNumber;
            }

            Debug.WriteLine("ERROR: Unauthenticated card or failed reading.");
            card.ReselectCard();
            throw new RfidException();
        }

        private void SetupEsp32Configuration()
        {
            Esp32.Configuration.SetPinFunction((int)MfRc522RfidReaderPinout.MOSI, Esp32.DeviceFunction.SPI1_MOSI);
            Esp32.Configuration.SetPinFunction((int)MfRc522RfidReaderPinout.MISO, Esp32.DeviceFunction.SPI1_MISO);
            Esp32.Configuration.SetPinFunction((int)MfRc522RfidReaderPinout.SDK, Esp32.DeviceFunction.SPI1_CLOCK);
        }

        private SpiDevice CreateSpiDevice()
        {
            var connection = new SpiConnectionSettings(1, (int)MfRc522RfidReaderPinout.SDA);
            connection.ClockFrequency = 5_000_000;

            return new SpiDevice(connection);
        }
    }
}
