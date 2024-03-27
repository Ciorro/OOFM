namespace OOFM.Core.Streaming;

public class SoundBuffer
{
    private readonly byte[] _buffer;
    private readonly object _bufferLock = new();

    private int _readPosition;
    private int _writePosition;

    public long Length { get; private set; }

    public SoundBuffer(int size)
    {
        _buffer = new byte[size];
    }

    private long _position;
    public long Position
    {
        get => _position;
        set
        {
            long diff = value - _position;

            if (_readPosition + diff > _buffer.Length ||
                _readPosition + diff < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            _position += diff;
            _readPosition += (int)diff;
        }
    }

    public int Read(byte[] buffer, int offset, int count)
    {
        lock (_bufferLock)
        {
            int bytesAvailable = _writePosition - _readPosition;
            count = Math.Min(count, bytesAvailable);

            Array.ConstrainedCopy(_buffer, _readPosition, buffer, offset, count);
            Position += count;

            return count;
        }
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        lock (_bufferLock)
        {
            if (count > _buffer.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            int overflow = _writePosition + count - _buffer.Length;
            if (overflow > 0)
            {
                DiscardBytes(overflow);
            }

            Array.ConstrainedCopy(buffer, offset, _buffer, _writePosition, count);

            _writePosition += count;
            Length += count;
        }
    }

    public long Seek(long offset, SeekOrigin seekOrigin)
    {
        lock (_bufferLock)
        {
            offset = seekOrigin switch
            {
                SeekOrigin.Begin => offset - Position,
                SeekOrigin.End => Length - offset - Position,
                _ => offset
            };

            return SeekInternal(offset);
        }
    }

    public void Clear()
    {
        lock (_bufferLock)
        {
            Array.Fill<byte>(_buffer, 0);

            Length = 0;
            _position = 0;
            _readPosition = 0;
            _writePosition = 0;
        }
    }

    private long SeekInternal(long offset)
    {
        if (offset < -_readPosition)
        {
            offset = -_readPosition;
        }

        if (offset > Length - Position)
        {
            offset = Length - Position;
        }

        Position += offset;
        return Position;
    }

    private void DiscardBytes(int count)
    {
        if (count <= 0)
        {
            return;
        }

        for (int i = count; i < _buffer.Length; i++)
        {
            _buffer[i - count] = _buffer[i];
            _buffer[i] = 0;
        }

        _readPosition -= count;
        _writePosition -= count;

        if (_readPosition < 0)
        {
            _position += -_readPosition;
            _readPosition = 0;
        }
    }
}
